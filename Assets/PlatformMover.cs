using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    Vector2 origPos;
    public Vector2 startingPos;
    public Vector2 endPos;
    public float startSpeed;
    public float endSpeed;

    public IEnumerator MoveStart()
    {
        origPos = transform.position;
        yield return StartCoroutine(MoveHelper(startSpeed, origPos + startingPos));
    }

    public IEnumerator MoveEnd()
    {
        origPos = transform.position;
        yield return StartCoroutine(MoveHelper(endSpeed, origPos + endPos));
    }

    private IEnumerator MoveHelper(float baseSpeed, Vector2 targetPos)
    {
        float elapsedTime = 0;

        while (elapsedTime < baseSpeed)
        {
            transform.position = Vector2.Lerp(origPos, targetPos, (elapsedTime / baseSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
}
