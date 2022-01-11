using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ballLauncher : MonoBehaviour
{
    // All of this shit needs a refresher

    // Turn this into the player with playerData?
    // Throw all the items on here so they are all accessible?
    public Transform parent;

    public PlayerMovement player;

    public GameObject ballPrefab;
    public SpriteRenderer sprite;

    public LineRenderer lineRend;
    private Vector2 startingPos;
    private Vector2 mousePos;
    private Vector2 shotDirection;

    public bool canShoot = true;
    public bool shotFired;
    public bool aim;
    public bool posUpdated;
    public float nextXPos;

    public float ballSpeed;
    public float currentHits;
    public float upgradeModifier;
    public int currentBalls;
    public int totalBalls;
    public float ballDelay;
    public LayerMask ballLayer;

    public GameObject numBallsText;
    public GameObject numUpgradeText;

    public LaunchType defaultLaunchEffect = new StraightShot(); 

    void OnEnable()
    {
        aim = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        shotFired = false;
        totalBalls = 2;
        //hitsToUpgrade = ballsToLaunch * upgradeModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // This handles the shooting
        // Potential issue, probably need some delay between clicking down and shooting
        // Don't want to send the ball with just a click

        if (canShoot)
        {
            if (Input.GetButtonDown("Fire2"))
                Shoot();
        }
    }

    IEnumerator LaunchDelay(float numToLaunch)
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation, parent);
        ball.GetComponent<ballObject>().Size(.25f);
        defaultLaunchEffect.Launch(ball, shotDirection);
        ball.name = "Ball " + parent.childCount + 1;

        yield return new WaitForSeconds(ballDelay);
    }

    IEnumerator SpeedUp()
    {
        while (Time.timeScale < 2f)
        {
            if (!shotFired)
                break;

            Time.timeScale = Time.timeScale + 0.05f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    Vector2 Round(Vector2 input)
    {
        // Multiply this by 10^n where n is the number of decimals
        return new Vector2(Mathf.Round(input.x * 1000f) / 1000f, Mathf.Round(input.y * 1000f) / 1000f);
    }

    void OnAim()
    {
        Debug.Log("OnAim");
        aim = !aim;
        if (!aim && canShoot)
        {
            Shoot();
        }
    }

    void Aim()
    {
        Debug.Log("Aiming");

        // Raycasting
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1, player.lookDir);

        Vector2 nextDirection;

        lineRend.startWidth = .25f;
        lineRend.endWidth = .25f;
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, transform.position);
        lineRend.SetPosition(1, hit.centroid); // Put aim target in here
    }

    void Shoot()
    {
        lineRend.positionCount = 0;
        shotFired = true;

        // Hide the sprite
        sprite.enabled = false;

        // For locked shot
        shotDirection = player.lookDir;

        // This is where the shot is launched
        StartCoroutine(LaunchDelay(1));
    }
}
