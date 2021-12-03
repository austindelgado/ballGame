using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    public GameObject player;
    public Animator animator;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public float damage = 20f;
    public bool inAnimation = false;
    public Vector2 lookDir;

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {

        }
    }

    void FixedUpdate()
    {
        if (!inAnimation)
            transform.rotation = Quaternion.LookRotation(Vector3.forward, lookDir);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            // Send ball other way
            collision.gameObject.GetComponent<ballObject>().currDirection = lookDir;
            collision.gameObject.GetComponent<ballObject>().Bounce();
            collision.gameObject.GetComponent<ballObject>().shotSpeed += 5f;
        }
    }

    void OnAttack()
    {
        animator.SetTrigger("attack");
        nextAttackTime = Time.time + 1f / attackRate;
    }

    void OnLook(InputValue value)
    {
        Debug.Log("OnLook");
        lookDir = value.Get<Vector2>();
    }
}
