using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballObject : MonoBehaviour
{
    public GameObject launchManager;
    public SpriteRenderer spriteRenderer;
    public LineRenderer lineRend;
    public Rigidbody2D rb;

    public Vector2 currDirection;
    public Vector2 nextDirection;
    public Vector2 nextPosition;
    public Vector2 lastPosition;
    public Vector2 newNormal;
    public GameObject nextHit;

    public bool isColliding;
    public bool bouncing;
    public bool starting = true;

    private float shotSpeed;
    public float startingHealth;
    private float currentHealth;
    public LayerMask ballLayer;

    public List<HitEffect> hitEffects;

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
        // Subscribe to blockBreak
        GameManager.manager.OnBlockBreak += this.Bounce;

        launchManager = GameObject.Find("Ball Launcher");
        shotSpeed = launchManager.GetComponent<ballLauncher>().ballSpeed;
        currentHealth = startingHealth;

        Bounce();
    }

    void OnDestroy()
    {
        GameManager.manager.OnBlockBreak -= this.Bounce;
    }

    void Update()
    {
        // Change color depending on HP
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

    void FixedUpdate()
    {
        if (!bouncing)
        {
            float step = shotSpeed * Time.fixedDeltaTime;
            transform.position = Vector2.MoveTowards(transform.position, nextPosition, step);
        }

        //if (nextPosition == (Vector2)transform.position) // I think this is too imprecise
        if (Vector2.Distance((Vector2)transform.position, nextPosition) < 0.0001f)
        {
            //Debug.Log("Actual: " + transform.position.ToString("F4") + ", Expected: " + nextPosition.ToString("F4"));
            transform.position = nextPosition;
            currDirection = nextDirection;
            Bounce();
        }
    }

    public void Bounce()
    {
        //Debug.Log("Bouncing");

        if (bouncing) return;
        bouncing = true;

        // Current projection
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, .5f, currDirection, 80f, ~ballLayer);

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
        RaycastHit2D bounce = Physics2D.CircleCast(hit.centroid, .5f, nextDirection, 80f, ~ballLayer);

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

        if (collision.gameObject.tag == "Block")
        {
            // EW! Doing this fixes hitting in between colliders but messes up corner hits
            // Find a better solution
            if (!collision.gameObject.transform.parent.gameObject.GetComponent<blockObject>().hitThisUpdate)
            {
                currentHealth = startingHealth;
                launchManager.GetComponent<ballLauncher>().currentHits++; // This can be decoupled and added to the OnBallHit

                foreach (HitEffect hitEffect in hitEffects)
                    hitEffect.Hit();

                // Global hit call goes here, pass in collision object
                //Debug.Log("Block Hit");
                GameManager.manager.BallHit(collision.gameObject.transform.parent.gameObject);
            }
        }
        else if (collision.gameObject.tag == "Enemy") // It only gets worse
        {
            if (!collision.gameObject.transform.parent.gameObject.transform.parent.GetComponent<blockObject>().hitThisUpdate)
            {
                currentHealth = startingHealth;
                launchManager.GetComponent<ballLauncher>().currentHits++;

                // Global hit call goes here, pass in collision object
                //Debug.Log("Block Hit");
                GameManager.manager.BallHit(collision.gameObject.transform.parent.gameObject.transform.parent.gameObject);
            }
        }
        else
        {
            currentHealth--;
            if (currentHealth <= 0)
                Destroy(gameObject);

            GameManager.manager.BallHit(collision.gameObject);
        }
    }

    Vector2 Round(Vector2 input)
    {
        // Multiply this by 10^n where n is the number of decimals
        return new Vector2(Mathf.Round(input.x * 1000f) / 1000f, Mathf.Round(input.y * 1000f) / 1000f);
    }
}
