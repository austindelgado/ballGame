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
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -player.lookDir);

        attackPos = (Vector2)player.transform.position + attackOffset * player.lookDir.normalized;

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

            Instantiate(swing, attackPos, transform.rotation);

            Debug.Log("Attacking");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPos, attackRange, layer);
            foreach (var hitCollider in hitColliders)
            {

                if (hitCollider.gameObject.tag == "Ball")
                {
                    // Send ball other way
                    if (GlobalData.Instance.split)
                    {
                        Debug.Log("Split hit");
                        if (hitCollider.gameObject.GetComponent<ballObject>().size > .25f) // Split ball if it's greater than .25
                        {
                            // Spawn two new balls after destroying old one
                            float size = hitCollider.gameObject.GetComponent<ballObject>().size * .5f;
                            float health = hitCollider.gameObject.GetComponent<ballObject>().currentHealth;
                            Vector2 pos = hitCollider.gameObject.transform.position; // Half of original size
                            Destroy(hitCollider.gameObject);

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
                            hitCollider.gameObject.GetComponent<ballObject>().currDirection = player.lookDir;
                            hitCollider.gameObject.GetComponent<ballObject>().Bounce();
                            hitCollider.gameObject.GetComponent<ballObject>().shotSpeed += 5f;
                        }
                    }
                    else
                    {
                        Debug.Log("Ball hit");
                        //hitCollider.gameObject.GetComponent<ballObject>().damage++;
                        hitCollider.gameObject.GetComponent<ballObject>().currDirection = player.lookDir;
                        hitCollider.gameObject.GetComponent<ballObject>().Bounce();
                        hitCollider.gameObject.GetComponent<ballObject>().shotSpeed += 5f;
                    }
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
