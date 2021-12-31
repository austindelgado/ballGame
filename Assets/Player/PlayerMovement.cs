using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    PlayerController controls;

    public Rigidbody2D rb;
    public Animator animator;

    private SpriteRenderer spriteRenderer;

    public Vector2 move;
    Vector2 mousePos;
    public Vector2 lookDir;
    public bool lookLeft;

    // This is not pretty, change this
    public ShopSlot currentShopSlot = null;

    public ballLauncher bLauncher;
    public MeleeWeapon mWeapon;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    // For input
    void Update()
    {
        Debug.Log(move.normalized);

        if (move != Vector2.zero)
            animator.SetInteger("Speed", 1);
        else
            animator.SetInteger("Speed", -1);

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

    // Actual movement
    void FixedUpdate()
    {
        //rb.MovePosition((Vector2)transform.position + move.normalized * GlobalData.Instance.playerSpeed * Time.deltaTime);

        Vector2 moveFactor = new Vector2(move.normalized.x * 40f * GlobalData.Instance.playerSpeed * Time.fixedDeltaTime, rb.velocity.y); // The 40f here is based off nothing but will need to change if the scale of the game changes

        rb.velocity = moveFactor;
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

    void OnInteract()
    {
        if (currentShopSlot)
            currentShopSlot.Purchase();
    }

    void OnEscape()
    {
        Destroy(GlobalData.Instance);
        SceneManager.LoadScene(0);
    }

    void OnMouseLook(InputValue value)
    {
        Debug.Log("OnMouseLook");
        lookDir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
    }

    public void ToggleShoot()
    {
        bLauncher.canShoot = !bLauncher.canShoot;
    }

    public void ToggleMelee()
    {
        mWeapon.canAttack = !mWeapon.canAttack;
    }
}
