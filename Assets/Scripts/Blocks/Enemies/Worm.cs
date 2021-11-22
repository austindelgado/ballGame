using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turn into cave specific worm? Only worm in cave?
public class Worm : blockObject
{
    public int minLength;
    public int maxLength;

    public Material wormHead;
    public Material wormBody;

    public GameObject head;
    public Vector2 headPosition;

    public GameObject tail;
    public Vector2 tailPosition;

    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Pick random head, maybe change to head closer to middle?

        float rng = Random.Range(.75f, 1.25f);


        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(3).gameObject;


        // Improve health code at some point please, turn into a single function in the blockObject class
        depth = piecePositions[0].y;

        // Check what this is being based off initially, seems like it's coming from the blockCount?

        // Not sure if depth mod is needed here, proably need to rewrite this
        currentHealth = Mathf.Max(1, Mathf.RoundToInt(depth * depthMod * rng));

        FindLowestY();

        SpawnText();
        UpdateBlocks();
        Texture();
    }

    public override void Texture()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<dynamicPiece>().Texture(wormBody);
        }

        head.gameObject.GetComponent<dynamicPiece>().Texture(wormHead);
    }

    public override void SpawnText()
    {
        textObject = Instantiate(textPrefab, head.transform.position, Quaternion.Euler(0, 0, 0), head.transform);
    }

    public override IEnumerator Move()
    {
        //Debug.Log("Worm move!");

        // Worm moves like a snake from snake
        // Head moves forward, pieces behind go to previous position of piece in front

        // Decide where the head is going
        headPosition = piecePositions[0];

        // Check the cardinal grid directions, find an open spot
        List<Vector2> possibleMoves = new List<Vector2>();

        if (headPosition.y - 1 > -1 && GridManager.manager.grid[(int)headPosition.x, (int)headPosition.y - 1] == null) // Check Up
        {
            possibleMoves.Add(new Vector2(0, -1));
        }
        if (headPosition.x + 1 < GridManager.manager.cols && GridManager.manager.grid[(int)headPosition.x + 1, (int)headPosition.y] == null) // Check Right
        {
            possibleMoves.Add(new Vector2(1, 0));
        }
        if (headPosition.y + 1 < GridManager.manager.rows && GridManager.manager.grid[(int)headPosition.x, (int)headPosition.y + 1] == null) // Check Down
        {
            possibleMoves.Add(new Vector2(0, 1));
        }
        if (headPosition.x - 1 > -1 && GridManager.manager.grid[(int)headPosition.x - 1, (int)headPosition.y] == null) // Check Left
        {
            possibleMoves.Add(new Vector2(-1, 0));
        }

        if (possibleMoves.Count == 0) // Worm trapped!
            yield break;

        // Pick random direction from open spot, maybe weigh them
        Vector2 nextMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
        //Debug.Log(nextMove);

        Vector2 nextPosition = new Vector2(transform.GetChild(0).position.x + (nextMove.x * GridManager.manager.blockSpacing), transform.GetChild(0).position.y - (nextMove.y * GridManager.manager.blockSpacing));
        Vector2 previousPosition;
        Vector2 previousPiecePosition = Vector2.zero;

        // Only need to update grid for first and last position

        // Give each dynamic block a move function like in terrainHelper
        // Find next spot for head
        // Call move on all pieces, passing in the next location



        // Things break when piecePositions go negative
        // Move  along with worm/
        // Get rid of  all together?

        // Loop through, store previous, move current piece to previous
        Coroutine[] coroutines = new Coroutine[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2 tempPiecePosition = piecePositions[i];
            previousPosition = transform.GetChild(i).position;

            if (i == 0)
                piecePositions[i] = piecePositions[i] + nextMove; 
            else
                piecePositions[i] = previousPiecePosition;

            coroutines[i] = StartCoroutine(transform.GetChild(i).GetComponent<dynamicPiece>().Move(nextPosition, moveSpeed));
            previousPiecePosition = tempPiecePosition;
            nextPosition = previousPosition;
        }
        for (int i = 0; i < coroutines.Length; i++)
            yield return coroutines[i];

        headPosition = piecePositions[0];
        tailPosition = previousPiecePosition;
        GridManager.manager.grid[(int)tailPosition.x, (int)tailPosition.y] = null;
        GridManager.manager.grid[(int)headPosition.x, (int)headPosition.y] = gameObject;

        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(3).gameObject;

        UpdateBlocks();
        FindLowestY();
        Texture();
    }

    void UpdateBlocks()
    {
        int numChildren = transform.childCount;

        for (int i = 0; i < numChildren; i++)
        {
            // True if a block is found
            bool top = false;
            bool right = false;
            bool bottom = false;
            bool left = false;

            GameObject pieceObj = transform.GetChild(i).gameObject;
            Vector2 piece = piecePositions[i];
            Vector2 pieceBefore;
            Vector2 pieceAfter;

            // Need to refer to grid for update blocks

            if (i != 0)
                pieceBefore = piecePositions[i - 1];
            else
                pieceBefore = new Vector2(-1000, -1000);

            if (i != numChildren - 1)
                pieceAfter = piecePositions[i + 1];
            else
                pieceAfter = new Vector2(-1000, -1000);

            int blockSpacing = GridManager.manager.blockSpacing;
            // Check Top
            if (piece + Vector2.down == pieceBefore || piece + Vector2.down == pieceAfter)
            {
                //Debug.Log("Before or after above");
                top = true;
            }

            // Check Right
            if (piece + Vector2.right == pieceBefore || piece + Vector2.right == pieceAfter)
            {
                //Debug.Log("Before or after right");
                right = true;
            }

            // Check Bottom
            if (piece + Vector2.up == pieceBefore || piece + Vector2.up == pieceAfter)
            {
                //Debug.Log("Before or after down");
                bottom = true;
            }

            // Check left
            if (piece + Vector2.left == pieceBefore || piece + Vector2.left == pieceAfter)
            {
                //Debug.Log("Before or after left");
                left = true;
            }

            // This is so ugly
            // Please improve this at some point

            // Destroy the child blocks
            //Destroy(transform.GetChild(i).gameObject);
            //GameObject newPiece;

            // Square
            if (top && right && bottom && left)
            {
                //Debug.Log("Normal surrounded block");
                pieceObj.GetComponent<dynamicPiece>().SetState(0, 1);
            }
            else if (right && left)
            {
                //Debug.Log("Normal horizontal block");
                pieceObj.GetComponent<dynamicPiece>().SetState(0, 1);
            }
            else if (top && bottom)
            {
                //Debug.Log("Normal vertical block");
                pieceObj.GetComponent<dynamicPiece>().SetState(0, 1);
            }

            // Turns
            else if (top && right)
            {
                //Debug.Log("Bottom left turn");
                pieceObj.GetComponent<dynamicPiece>().SetState(1, 2);
            }
            else if (right && bottom)
            {
                //Debug.Log("Top left turn");
                pieceObj.GetComponent<dynamicPiece>().SetState(1, 1);
            }
            else if (bottom && left)
            {
                //Debug.Log("Top right turn");
                pieceObj.GetComponent<dynamicPiece>().SetState(1, 4);
            }
            else if (left && top)
            {
                //Debug.Log("Bottom right turn");
                pieceObj.GetComponent<dynamicPiece>().SetState(1, 3);
            }

            // Top
            else if (bottom)
            {
                //Debug.Log("Upright end piece");
                pieceObj.GetComponent<dynamicPiece>().SetState(2, 1);
            }
            else if (left)
            {
                //Debug.Log("Right end piece");
                pieceObj.GetComponent<dynamicPiece>().SetState(2, 4);
            }
            else if (top)
            {
                //Debug.Log("bottom end piece");
                pieceObj.GetComponent<dynamicPiece>().SetState(2, 3);
            }
            else if (right)
            {
                //Debug.Log("left end piece");
                pieceObj.GetComponent<dynamicPiece>().SetState(2, 2);
            }
            else
            {
                // Solo block, add a rounded solo block? Circle maybe?
                pieceObj.GetComponent<dynamicPiece>().SetState(2, 1);
            }
        }

        textObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
