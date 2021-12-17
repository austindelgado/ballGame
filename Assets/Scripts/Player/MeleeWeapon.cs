using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    public PlayerMovement player;
    public Animator animator;

    public GameObject ballPrefab; // These needs to be moved
    public Transform parent;

    public ParticleSystem swing;

    private float timeBtwAttack;
    public bool canAttack;
    public bool offCooldown;
    private Vector2 attackPos;
    public float attackOffset;
    public LayerMask layer;
    public float attackRange;

    public int angle;
    private Quaternion angleOffsetL;
    private Quaternion angleOffsetR;

    void Start()
    {
        angleOffsetL = Quaternion.AngleAxis(angle, Vector3.forward);
        angleOffsetR = Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("playerAttack"))
            transform.rotation = Quaternion.LookRotation(Vector3.forward, -player.lookDir);

        //attackPos = (Vector2)player.transform.position + attackOffset * player.lookDir.normalized;

        if (timeBtwAttack <= 0)
            offCooldown = true;
        else
            timeBtwAttack -= Time.deltaTime;
    }

    void OnAttack()
    {
        if (canAttack && offCooldown)
        {
            // Start cooldown
            offCooldown = false;
            timeBtwAttack = GlobalData.Instance.attackCD;

            //angle = angle * -1;
            //angleOffset = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.Log("Attacking");
            animator.SetTrigger("attack");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bat collision");
        if (collision.gameObject.tag == "Ball")
        {
            // Send ball other way
            if (GlobalData.Instance.split)
            {
                Debug.Log("Split hit");
                if (collision.gameObject.GetComponent<ballObject>().size > .25f) // Split ball if it's greater than .25
                {
                    // Spawn two new balls after destroying old one
                    float size = collision.gameObject.GetComponent<ballObject>().size * .5f;
                    float health = collision.gameObject.GetComponent<ballObject>().currentHealth;
                    Vector2 pos = collision.gameObject.transform.position; // Half of original size
                    Destroy(collision.gameObject);

                    // Ball 1
                    GameObject ball1 = Instantiate(ballPrefab, pos, transform.rotation, parent);
                    new StraightShot().Launch(ball1, angleOffsetR * player.lookDir);
                    ball1.GetComponent<ballObject>().Size(size);
                    ball1.GetComponent<ballObject>().currentHealth = health;

                    // Ball 2
                    GameObject ball2 = Instantiate(ballPrefab, pos, transform.rotation, parent);
                    new StraightShot().Launch(ball2, angleOffsetL * player.lookDir);
                    ball2.GetComponent<ballObject>().Size(size);
                    ball2.GetComponent<ballObject>().currentHealth = health;
                }
                else
                {
                    collision.gameObject.GetComponent<ballObject>().currDirection = player.lookDir;
                    collision.gameObject.GetComponent<ballObject>().Bounce();
                    collision.gameObject.GetComponent<ballObject>().shotSpeed += 5f;
                }
            }
            else
            {
                Debug.Log("Ball hit");
                collision.gameObject.GetComponent<ballObject>().damage++;
                collision.gameObject.GetComponent<ballObject>().shotSpeed += 5f;
                collision.gameObject.GetComponent<ballObject>().Hit(player.lookDir);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
