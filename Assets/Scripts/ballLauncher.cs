using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // REMOVE THIS AT SOME POINT
using UnityEngine;
using TMPro;

public class ballLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    public SpriteRenderer sprite;

    public LineRenderer lineRend;
    private Vector2 startingPos;
    private Vector2 mousePos;
    private Vector2 shotDirection;

    public bool shotFired;
    public bool posUpdated;
    public float nextXPos;

    public float ballSpeed;
    public float currentHits;
    public float hitsToUpgrade;
    public float upgradeModifier;
    public float ballsToLaunch;
    public float ballDelay;

    public GameObject numBallsText;
    public GameObject numUpgradeText;

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

        if (Input.GetKeyDown("space"))
        {
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);

            /*
            if (GlobalData.Instance.debugMode)
                GlobalData.Instance.debugMode = false;
            else
                GlobalData.Instance.debugMode = true;
            */
        }

        if (!shotFired && Input.GetMouseButton(0))
        {
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            shotDirection = (worldPosition - (Vector2)transform.position).normalized;

            // Raycasting
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, .5f, shotDirection);

            // Debug drawings
            if (GlobalData.Instance.debugMode)
            {
                Vector2 unchangedDirection = Vector2.Reflect(shotDirection, hit.normal).normalized;

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
                Vector2 nextDirection = Vector2.Reflect(shotDirection, newNormal).normalized;

                Debug.DrawRay(transform.position, shotDirection * 80f, Color.black); // Shot
                Debug.DrawRay(transform.position, hit.centroid - (Vector2)transform.position, Color.red); // Shot?
                Debug.DrawRay(hit.centroid, hit.normal, Color.white); // Hit normal
                Debug.DrawRay(hit.centroid, newNormal, Color.magenta); // Rounded Hit normal
                Debug.DrawRay(hit.centroid, unchangedDirection, Color.green); // Unchanged Bounce
                Debug.DrawRay(hit.centroid, nextDirection, Color.blue); // Changed Bounce
            }

            // Hiding this for now, will revisit
            /*
            Vector2 aimTarget;

            float distance1 = Vector2.Distance((Vector2)transform.position, hit.centroid);
            float distance2 = Vector2.Distance((Vector2)transform.position, (Vector2)transform.position + shotDirection * 10f);

            Debug.Log(shotDirection);
            Debug.Log("Hit distance: " + distance1 + ", max distance: " + distance2);

            if (distance1 < distance2)
                aimTarget = hit.centroid;
            else
                aimTarget = (Vector2)transform.position + shotDirection * 10f;
            */

            // Draws Line
            lineRend.positionCount = 2;
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, hit.centroid); // Put aim target in here
        }
        else if (!shotFired && Input.GetMouseButtonUp(0))
        {
            lineRend.positionCount = 0;
            shotFired = true;

            // Hide the sprite
            sprite.enabled = false;

            // This is where the shot is launched
            StartCoroutine(LaunchDelay(ballsToLaunch));

            // Speed up time gradually if not in debug mode
            //if (!GlobalData.Instance.debugMode)
                StartCoroutine(SpeedUp());
        }


        // Get next shot ready
        if (shotFired && transform.childCount == 0)
        {
            shotFired = false;
            posUpdated = false;
            transform.position = new Vector2(nextXPos, transform.position.y);

            GridManager.manager.MoveBlocks(false);
            GridManager.manager.MoveEnemies();
            Time.timeScale = 1.0f;
            sprite.enabled = true;
        }

        // Update things
        numBallsText.GetComponent<TMP_Text>().text = ballsToLaunch.ToString();
        numUpgradeText.GetComponent<TMP_Text>().text = (hitsToUpgrade - currentHits).ToString();
        if (currentHits >= hitsToUpgrade)
        {
            ballsToLaunch++;
            currentHits = 0;

            // Need formula to get next hitsToUpgrade
            hitsToUpgrade = ballsToLaunch * upgradeModifier;

            if (shotFired)
                StartCoroutine(LaunchDelay(1));
        }
    }

    IEnumerator LaunchDelay(float numToLaunch)
    {
        for (int i = 0; i < numToLaunch; i++)
        {
            GameObject ball = Instantiate(ballPrefab, transform);
            ball.GetComponent<ballObject>().currDirection = shotDirection.normalized;
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
}
