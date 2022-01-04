using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballObject : MonoBehaviour
{
    public GameObject launchManager;
    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRend;
    public Rigidbody2D rb;
    public CircleCollider2D circCollider;

    public Vector2 currDirection;
    public Vector2 nextDirection;
    public Vector2 nextPosition;
    public Vector2 lastPosition;
    public Vector2 newNormal;
    public GameObject nextHit;

    public bool isColliding;
    public bool bouncing;
    public bool starting = true;

    public float shotSpeed;
    public float size;
    public int damage;
    public float startingHealth;
    public float currentHealth;
    public LayerMask ballLayer;

    public Transform popUpPref; // Please move, look at CodeMonkey video

    [Header("HP Colors")]
    public Color fullHealth;
    public Color five;
    public Color four;
    public Color three;
    public Color two;
    public Color one;

    // Start is called before the first frame update
    void Start()
    {
        shotSpeed = 8;
        startingHealth = 7;

        if (currentHealth == 0)
            currentHealth = startingHealth;

        // Change size for circleCast but change scale on ball
        //size = GlobalData.Instance.ballSize;
        damage = 1;
        //gameObject.transform.localScale = new Vector3(size * 2, size * 2, 1);

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = currDirection.normalized * shotSpeed;

        //Bounce();
    }

    void Update()
    {
        // // Change color depending on HP
        if (currentHealth > 5)
            spriteRenderer.color = fullHealth;
        else if (currentHealth == 5)
            spriteRenderer.color = five;
        else if (currentHealth == 4)
            spriteRenderer.color = four;
        else if (currentHealth == 3)
            spriteRenderer.color = three;
        else if (currentHealth == 2)
            spriteRenderer.color = two;
        else if (currentHealth == 1)
            spriteRenderer.color = one;
    }

    public void Bounce()
    {
        //Debug.Log("Bouncing");

        if (bouncing) return;
        bouncing = true;

        // Current projection
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, size, currDirection, 80f, ~ballLayer);

        // Holedown rounds this to nearest 45 degree angle, let's try that
        // Take normal and round to nearest 45 degree
        // This is not working properly
        float roundAngle = 45 * Mathf.Deg2Rad;
        float angle = (float)Mathf.Atan2(hit.normal.y, hit.normal.x);

        if (angle % roundAngle != 0)
        {
            float newAngle = (float)Mathf.Round(angle / roundAngle) * roundAngle;
            newNormal = new Vector2((float)Mathf.Cos(newAngle), (float)Mathf.Sin(newAngle));
        }
        else
        {
            newNormal = hit.normal.normalized;
        }
        
        Debug.DrawRay(hit.centroid, hit.normal, Color.white, 3f);
        Debug.DrawRay(hit.centroid, newNormal, Color.red, 3f);

        // Bounce projection
        nextDirection = Round(Vector2.Reflect(currDirection, hit.normal).normalized);
        //Debug.Log(nextDirection.ToString("F4"));

        nextPosition = hit.centroid;
        nextHit = hit.collider.gameObject;
        RaycastHit2D bounce = Physics2D.CircleCast(hit.centroid, size, nextDirection, 80f, ~ballLayer);

        // Draw lines here
        if (GlobalData.Instance.debugMode)
        {
            lineRend.positionCount = 3;
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, hit.centroid);
            lineRend.SetPosition(2, bounce.centroid);

            // Projection line
            Debug.DrawLine(transform.position, hit.centroid, Color.red, 5f);

            // Expected bounce
            Debug.DrawLine(hit.centroid, bounce.centroid, Color.blue, 5f);
        }

        bouncing = false;
    }

    // Sometimes getting called twice
    // Should this even be here? Should blocks be responsible for collisions?
    void OnTriggerEnter2D(Collider2D collision)
    {
        //currDirection = nextDirection;
        //Bounce();

        // Gotta clean this up soon

        if (collision.gameObject.tag == "Block")
        {
            // EW! Doing this fixes hitting in between colliders but messes up corner hits
            // Find a better solution
            if (!collision.gameObject.transform.parent.gameObject.GetComponent<blockObject>().hitThisUpdate)
            {
                if (collision.gameObject.GetComponent<pieceObject>().spikey == true)
                {
                    currentHealth -= 7;
                    if (currentHealth <= 0)
                        Destroy(gameObject);
                    return;
                }

                currentHealth = startingHealth;
                launchManager.GetComponent<ballLauncher>().currentHits++; // This can be decoupled and added to the OnBallHit

                // Global hit call goes here, pass in collision object
                //Debug.Log("Block Hit");
                collision.gameObject.transform.parent.GetComponent<blockObject>().AddDamage(damage);
                GameEvents.current.BallHit(collision.gameObject.transform.parent.gameObject);
            }
        }
        else if (collision.gameObject.tag == "Enemy") // It only gets worse
        {
            if (!collision.gameObject.transform.parent.gameObject.transform.parent.GetComponent<blockObject>().hitThisUpdate)
            {
                currentHealth = startingHealth;

            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Bat hit!");
        }
        else
        {
            currentHealth--;
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Ball collision");

        Vector2 towardsCollision = collision.contacts[0].point - (Vector2)transform.position;
        Ray2D ray = new Ray2D(transform.position, towardsCollision);
 
        // raycast for bricks
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1f, ~ballLayer);
        if(collision.gameObject.tag == "Block")
        {
            if (collision.gameObject.GetComponent<pieceObject>().spikey == true)
            {
                currentHealth -= 7;
                if (currentHealth <= 0)
                    Destroy(gameObject);
                return;
            }

            currentHealth = startingHealth;
            launchManager.GetComponent<ballLauncher>().currentHits++; // This can be decoupled and added to the OnBallHit

            // Global hit call goes here, pass in collision object
            //Debug.Log("Block Hit");
            collision.gameObject.transform.parent.GetComponent<blockObject>().AddDamage(damage);
            GameEvents.current.BallHit(collision.gameObject.transform.parent.gameObject);
        }
        else if (collision.gameObject.tag == "Enemy") // It only gets worse
        {
            currentHealth = startingHealth;
            DamagePopUp.Create(transform.position, damage, popUpPref);
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Bat hit!");
        }
        else
        {
            currentHealth--;
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
    }

    public void Hit(Vector2 hitDirection)
    {
        rb.velocity = hitDirection.normalized * shotSpeed;
    }

    Vector2 Round(Vector2 input)
    {
        // Multiply this by 10^n where n is the number of decimals
        return new Vector2(Mathf.Round(input.x * 1000f) / 1000f, Mathf.Round(input.y * 1000f) / 1000f);
    }

    public void Size(float newSize)
    {
        size = newSize;
        if (size < .25f)
            size = .25f;
        gameObject.transform.localScale = new Vector3(size * 2, size * 2, 1);
    }
}
