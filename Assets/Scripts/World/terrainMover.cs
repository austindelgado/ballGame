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

    public IEnumerator Move(int worldSpacing, float baseSpeed)
    {
        targetPos = new Vector2(transform.position.x, transform.position.y - worldSpacing * depthMod);

        if (!isMoving)
            yield return StartCoroutine(MoveHelper(baseSpeed));
    }

    private IEnumerator MoveHelper(float baseSpeed)
    {
        //Debug.Log("Speed of " + gameObject.name + ": " + baseSpeed);
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
