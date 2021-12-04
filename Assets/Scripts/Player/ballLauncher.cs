using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ballLauncher : MonoBehaviour
{
    // All of this shit needs a refresher

    // Turn this into the player with playerData?
    // Throw all the items on here so they are all accessible?
    public Transform parent;

    public PlayerController controls;

    public PlayerMovement player;

    public GameObject ballPrefab;
    public SpriteRenderer sprite;

    public LineRenderer lineRend;
    private Vector2 startingPos;
    private Vector2 mousePos;
    private Vector2 shotDirection;

    public bool shotFired;
    public bool aim;
    public bool posUpdated;
    public float nextXPos;

    public float ballSpeed;
    public float currentHits;
    public float hitsToUpgrade;
    public float upgradeModifier;
    public float ballsToLaunch;
    public float ballDelay;
    public LayerMask ballLayer;

    public GameObject numBallsText;
    public GameObject numUpgradeText;

    public LaunchType defaultLaunchEffect = new StraightShot(); 

    void OnEnable()
    {
        controls = new PlayerController();
        aim = false;
        controls.Gameplay.Aim.started += context => aim = true;
        controls.Gameplay.Aim.performed += context => aim = true;
        controls.Gameplay.Aim.canceled += context => aim = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        shotFired = false;
        ballsToLaunch = GlobalData.Instance.ballsToLaunch;
        hitsToUpgrade = ballsToLaunch * upgradeModifier;
    }

    // Update is called once per frame
    void Update()
    {
        // This handles the shooting
        // Potential issue, probably need some delay between clicking down and shooting
        // Don't want to send the ball with just a click

        if (aim)
            Aim();

        if (GameManager.manager.state == GameState.PLAYERTURN)
        {
            // Get next shot ready
            if (shotFired && parent.childCount == 0)
            {
                shotFired = false;
                posUpdated = false;
                Time.timeScale = 1.0f;
                sprite.enabled = true;

                // Player turn over, call enemyTurn
                GameManager.manager.PlayerTurnOver();
            }

            // Update things
            numBallsText.GetComponent<TMP_Text>().text = GlobalData.Instance.ballsToLaunch.ToString();
            numUpgradeText.GetComponent<TMP_Text>().text = (hitsToUpgrade - currentHits).ToString();
            if (currentHits >= hitsToUpgrade)
            {
                GlobalData.Instance.ballsToLaunch++;
                currentHits = 0;

                // Need formula to get next hitsToUpgrade
                hitsToUpgrade = GlobalData.Instance.ballsToLaunch * upgradeModifier;

                if (shotFired)
                    StartCoroutine(LaunchDelay(1)); 
            }
        }
    }

    IEnumerator LaunchDelay(float numToLaunch)
    {
        for (int i = 0; i < numToLaunch; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation, parent);

            defaultLaunchEffect.Launch(ball, shotDirection);
            ball.name = "Ball " + (i + 1);

            yield return new WaitForSeconds(ballDelay);
        }
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
        if (!aim)
        {
            Shoot();
        }
    }

    void Aim()
    {
        Debug.Log("Aiming");

        if (!shotFired && GameManager.manager.state == GameState.PLAYERTURN)
        {
            // Raycasting
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, .5f, player.lookDir);

            Vector2 nextDirection;

            lineRend.positionCount = 2;
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, hit.centroid); // Put aim target in here

            // Debug drawings
            if (GlobalData.Instance.aimIncrease)
            {
                lineRend.positionCount += 1;

                Vector2 unchangedDirection = Vector2.Reflect(player.lookDir, hit.normal).normalized;

                float roundAngle = 45 * Mathf.Deg2Rad;
                float angle = (float)Mathf.Atan2(hit.normal.y, hit.normal.x);
                Vector2 newNormal;

                if (angle % roundAngle != 0)
                {
                    float newAngle = (float)Mathf.Round(angle / roundAngle) * roundAngle;
                    newNormal = new Vector2((float)Mathf.Cos(newAngle), (float)Mathf.Sin(newAngle));
                }
                else
                {
                    newNormal = hit.normal.normalized;
                }

                nextDirection = Round(Vector2.Reflect(player.lookDir, hit.normal).normalized);
                lineRend.SetPosition(2, hit.centroid + nextDirection * 2f);

                //Debug.DrawRay(transform.position, player.lookDir * 80f, Color.black); // Shot
                //Debug.DrawRay(transform.position, hit.centroid - (Vector2)transform.position, Color.red); // Shot?
                //Debug.DrawRay(hit.centroid, hit.normal, Color.white); // Hit normal
                //Debug.DrawRay(hit.centroid, newNormal, Color.magenta); // Rounded Hit normal
                //Debug.DrawRay(hit.centroid, unchangedDirection, Color.green); // Unchanged Bounce
                //Debug.DrawRay(hit.centroid, nextDirection, Color.blue); // Changed Bounce
            }
        }
    }

    void Shoot()
    {
        if (!shotFired && GameManager.manager.state == GameState.PLAYERTURN)
        {
            lineRend.positionCount = 0;
            shotFired = true;

            // Hide the sprite
            sprite.enabled = false;

            // For locked shot
            shotDirection = player.lookDir;

            // This is where the shot is launched
            StartCoroutine(LaunchDelay(GlobalData.Instance.ballsToLaunch));
        }
    }

    void OnShoot()
    {

    }
}
