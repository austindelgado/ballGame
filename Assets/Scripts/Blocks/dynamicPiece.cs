using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dynamicPiece : MonoBehaviour
{
    public bool lockCheck;
    public int currentType = 0;
    public int currentRotation = 1;

    public bool isMoving;

    // Set state to block type as well as rotates block
        // Takes in two int, blockType and rotation
        // Block Type: 0 - blockPiece, 1 - EndPiece, 2 - TurnPiece
        // Rotations: 1 - Default, 2 - 90, 3 - 180, 4 - 270
    public void SetState(int blockType, int rotation)
    {
        // Handle blockType
        if (blockType != currentType)
        {
            transform.GetChild(blockType).gameObject.SetActive(true);
            transform.GetChild(currentType).gameObject.SetActive(false);
            currentType = blockType;
        }

        // Handle rotation - Maybe break off into separate function?
        if (rotation != currentRotation)
        {
            if (rotation == 1)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (rotation == 2)
                transform.rotation = Quaternion.Euler(0, 0, 90);
            else if (rotation == 3)
                transform.rotation = Quaternion.Euler(0, 0, 180);
            else if (rotation == 4)
                transform.rotation = Quaternion.Euler(0, 0, 270);

            currentRotation = rotation;
        }
    }

    public void Texture(Material mat)
    {
        transform.GetChild(currentType).gameObject.GetComponent<SpriteRenderer>().material = mat;
    }

    public IEnumerator Move(Vector2 targetPos, float baseSpeed)
    {
        if (!isMoving)
            yield return StartCoroutine(MoveHelper(targetPos, baseSpeed));
    }

    private IEnumerator MoveHelper(Vector2 targetPos, float baseSpeed)
    {
        isMoving = true;

        float elapsedTime = 0;

        Vector2 origPos = transform.position;

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
