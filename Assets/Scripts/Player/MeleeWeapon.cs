using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float attackCD = .5f;
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

        if (Input.GetButton("Fire1"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (canAttack && offCooldown)
        {
            // Start cooldown
            offCooldown = false;
            timeBtwAttack = attackCD;

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
            Debug.Log("Ball hit");
            collision.gameObject.GetComponent<ballObject>().damage++;
            collision.gameObject.GetComponent<ballObject>().shotSpeed += 2f;
            collision.gameObject.GetComponent<ballObject>().Hit(player.lookDir);
        }
    }
}
