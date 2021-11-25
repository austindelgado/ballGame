using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move all of launch and coroutine into here?

public abstract class LaunchType 
{
    public abstract void Launch(GameObject ball, Vector2 shotDir);
}

public class StraightShot : LaunchType
{
    public override void Launch(GameObject ball, Vector2 shotDir)
    {
        Debug.Log("Default Launch!");
        ball.GetComponent<ballObject>().currDirection = shotDir.normalized;
    }
}

public class TripleShot : LaunchType
{
    public override void Launch(GameObject ball, Vector2 shotDir)
    {
        GameObject bLauncher = GameObject.Find("Ball Launcher");

        Debug.Log("Triple Shot Launch!");
        ball.GetComponent<ballObject>().currDirection = shotDir.normalized;

        GameObject secondBall = GameObject.Instantiate(ball, ball.transform.position, ball.transform.rotation, bLauncher.transform);
        secondBall.GetComponent<ballObject>().currDirection = new Vector2(shotDir.x * Mathf.Cos(10 * Mathf.Deg2Rad) - shotDir.y * Mathf.Sin(10 * Mathf.Deg2Rad), shotDir.x * Mathf.Sin(10 * Mathf.Deg2Rad) + shotDir.y * Mathf.Cos(10 * Mathf.Deg2Rad));
        //secondBall.GetComponent<ballObject>().hitEffects = bLauncher.GetComponent<ballLauncher>().defaultHitEffects;

        GameObject thirdBall = GameObject.Instantiate(ball, ball.transform.position, ball.transform.rotation, bLauncher.transform);
        thirdBall.GetComponent<ballObject>().currDirection = new Vector2(shotDir.x * Mathf.Cos(-10 * Mathf.Deg2Rad) - shotDir.y * Mathf.Sin(-10 * Mathf.Deg2Rad), shotDir.x * Mathf.Sin(-10 * Mathf.Deg2Rad) + shotDir.y * Mathf.Cos(-10 * Mathf.Deg2Rad));
        //thirdBall.GetComponent<ballObject>().hitEffects = bLauncher.GetComponent<ballLauncher>().defaultHitEffects;
    }
}

public class DoubleShot : LaunchType
{
    public override void Launch(GameObject ball, Vector2 shotDir)
    {
        Debug.Log("Double Shot Launch!");

        GameObject bLauncher = GameObject.Find("Ball Launcher");

        Vector2 startingPos = ball.transform.position;

        ball.transform.position = startingPos + Vector2.Perpendicular(shotDir) * .75f;
        ball.GetComponent<ballObject>().currDirection = shotDir.normalized;

        GameObject secondBall = GameObject.Instantiate(ball, startingPos - Vector2.Perpendicular(shotDir) * .75f, ball.transform.rotation, bLauncher.transform);
        secondBall.GetComponent<ballObject>().currDirection = shotDir.normalized;
        //secondBall.GetComponent<ballObject>().hitEffects = bLauncher.GetComponent<ballLauncher>().defaultHitEffects;
    }
}

public class WackyShot : LaunchType
{
    public override void Launch(GameObject ball, Vector2 shotDir)
    {
        float rng = Random.value;

        if (rng < .33)
            new StraightShot().Launch(ball, shotDir);
        else if (rng < .66)
            new DoubleShot().Launch(ball, shotDir);
        else
            new TripleShot().Launch(ball, shotDir);
    }
}

public class SprayShot : LaunchType
{
    public override void Launch(GameObject ball, Vector2 shotDir)
    {
        float shotAngle = Random.Range(-15, 15);

        ball.GetComponent<ballObject>().currDirection = new Vector2(shotDir.x * Mathf.Cos(shotAngle * Mathf.Deg2Rad) - shotDir.y * Mathf.Sin(shotAngle * Mathf.Deg2Rad), shotDir.x * Mathf.Sin(shotAngle * Mathf.Deg2Rad) + shotDir.y * Mathf.Cos(shotAngle * Mathf.Deg2Rad));
    }
}