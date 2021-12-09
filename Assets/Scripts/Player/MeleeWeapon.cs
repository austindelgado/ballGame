using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    public PlayerMovement player;
    public Animator animator;

    public ParticleSystem swing;

    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public bool canAttack;
    public bool offCooldown;
    private Vector2 attackPos;
    public float attackOffset;
    public LayerMask layer;
    public float attackRange;

    public int angle;
    public Quaternion angleOffset;

    void Start()
    {
        //angleOffset = Quaternion.AngleAxis(angle, Vector3.forward);
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
                // Send ball other way
                if (hitCollider.gameObject.tag == "Ball")
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
