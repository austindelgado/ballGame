using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    private SpriteRenderer spriteRenderer;

    public Vector2 move;
    Vector2 mousePos;
    public Vector2 lookDir;
    public bool lookLeft;

    public float playerSpeed;

    // // This is not pretty, change this
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

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (Input.GetKeyDown("escape"))
            OnEscape();
    }

    // Actual movement
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + move.normalized * playerSpeed * Time.fixedDeltaTime);
    }

    void OnMove()
    {
        Debug.Log("OnMove");
    }

    void OnLook()
    {
        Debug.Log("OnLook");
    }

    void OnInteract()
    {
        // if (currentShopSlot)
        //     currentShopSlot.Purchase();
    }

    void OnEscape()
    {
        //Destroy(GlobalData.Instance);
        Debug.Log("Application quit");
        Application.Quit();
    }

    void OnMouseLook()
    {
        Debug.Log("OnMouseLook");
        //lookDir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
    }

    public void ToggleShoot()
    {
        //bLauncher.canShoot = !bLauncher.canShoot;
    }

    public void ToggleMelee()
    {
        //mWeapon.canAttack = !mWeapon.canAttack;
    }
}
