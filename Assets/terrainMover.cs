using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainMover : MonoBehaviour
{
    bool isMoving;
    Vector2 origPos;
    Vector2 targetPos;
    public float depthMod;
    private float baseSpeed;

    public void Move(int worldSpacing, float baseSpeed)
    {
        targetPos = new Vector2(transform.position.x, transform.position.y - worldSpacing * depthMod);
        Debug.Log(targetPos);

        // Change this to a smooth lerp, this is where we'll get nice movement from
        if (!isMoving)
            StartCoroutine(MoveHelper(baseSpeed));
    }

    private IEnumerator MoveHelper(float baseSpeed)
    {
        Debug.Log("Speed of " + gameObject.name + ": " + baseSpeed);
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;

        while (elapsedTime < baseSpeed)
        {
            transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / baseSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
}
