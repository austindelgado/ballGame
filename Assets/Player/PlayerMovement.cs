using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerController controls;

    public float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    private SpriteRenderer spriteRenderer;

    public Vector2 move;
    Vector2 mousePos;
    public Vector2 lookDir;
    public bool lookLeft;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    // For input
    void Update()
    {
        rb.MovePosition(rb.position + move.normalized * moveSpeed * Time.fixedDeltaTime);

        if (move != Vector2.zero)
            animator.SetInteger("Speed", 1);
        else
            animator.SetInteger("Speed", -1);
    }

    // Actual movement
    void FixedUpdate()
    {
        if (lookDir.x < 0)
        {
            spriteRenderer.flipX = false;
            lookLeft = true;
        }
        else
        {
            spriteRenderer.flipX = true;
            lookLeft = false;
        }
    }

    void OnMove(InputValue value)
    {
        Debug.Log("OnMove");
        move = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        Debug.Log("OnLook");
        lookDir = value.Get<Vector2>();
    }

    void OnMouseLook(InputValue value)
    {
        Debug.Log("OnMouseLook");
        lookDir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
    }

}
