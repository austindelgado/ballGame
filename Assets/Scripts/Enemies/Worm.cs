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

    // Start is called before the first frame update
    void Start()
    {
        // Pick random head, maybe change to head closer to middle?

        float rng = Random.Range(.75f, 1.25f);


        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(3).gameObject;


        // Improve health code at some point please, turn into a single function in the blockObject class
        depth = -transform.position.y;

        // Check what this is being based off initially, seems like it's coming from the blockCount?

        // Not sure if depth mod is needed here, proably need to rewrite this
        currentHealth = Mathf.Max(1, Mathf.RoundToInt(((depth + GlobalData.Instance.blockHealthStartMod) * depthMod) * startingHealth * rng));

        Texture();
        SpawnText();
    }

    public override void Texture()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SpriteRenderer>().material = wormBody;
        }

        head.GetComponent<SpriteRenderer>().material = wormHead;
    }

    public override void SpawnText()
    {
        Debug.Log("Spawning Text!");
        textObject = Instantiate(textPrefab, head.transform.position, Quaternion.Euler(0, 0, 0), head.transform);
    }

    public override void Move()
    {
        Debug.Log("Worm move!");

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
            return;

        // Pick random direction from open spot, maybe weigh them
        Vector2 nextMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
        //Debug.Log(nextMove);

        Vector2 nextPosition = new Vector2(transform.GetChild(0).position.x + (nextMove.x * GridManager.manager.blockSpacing), transform.GetChild(0).position.y - (nextMove.y * GridManager.manager.blockSpacing));
        Vector2 previousPosition;
        Vector2 previousPiecePosition = Vector2.zero;

        // Only need to update grid for first and last position


        // Things break when piecePositions go negative
        // Move  along with worm/
        // Get rid of  all together?

        // Loop through, store previous, move current piece to previous
        for (int i = 0; i < transform.childCount; i++)
        {
            Vector2 tempPiecePosition = piecePositions[i];
            previousPosition = transform.GetChild(i).position;

            if (i == 0)
                piecePositions[i] = piecePositions[i] + nextMove; 
            else
                piecePositions[i] = previousPiecePosition;

            transform.GetChild(i).transform.position = nextPosition;
            previousPiecePosition = tempPiecePosition;
            nextPosition = previousPosition;
        }

        headPosition = piecePositions[0];
        tailPosition = previousPiecePosition;
        GridManager.manager.grid[(int)tailPosition.x, (int)tailPosition.y] = null;
        GridManager.manager.grid[(int)headPosition.x, (int)headPosition.y] = gameObject;

        head = transform.GetChild(0).gameObject;
        tail = transform.GetChild(3).gameObject;

        UpdateBlocks();
        Texture();
        SpawnText();
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

            // To fix worms forming squares, check that it's bordering piece before and after in list
            // To fix worms forming squares, check that it's bordering piece before and after in list

            Vector2 piece = transform.GetChild(i).position;
            Vector2 pieceBefore;
            Vector2 pieceAfter;

            if (i != 0)
                pieceBefore = transform.GetChild(i - 1).position;
            else
                pieceBefore = new Vector2(-1000, -1000);

            if (i != numChildren - 1)
                pieceAfter = transform.GetChild(i + 1).position;
            else
                pieceAfter = new Vector2(-1000, -1000);

            //Debug.Log(transform.GetChild(i).name);
            //Debug.Log("Before: " + pieceBefore);
            //Debug.Log("Piece: " + piece);
            //Debug.Log("After: " + pieceAfter);


            int blockSpacing = GridManager.manager.blockSpacing;
            // Check Top
            if (piece + Vector2.up * blockSpacing == pieceBefore || piece + Vector2.up * blockSpacing == pieceAfter)
            {
                //Debug.Log("Before or after above");
                top = true;
            }

            // Check Right
            if (piece + Vector2.right * blockSpacing == pieceBefore || piece + Vector2.right * blockSpacing == pieceAfter)
            {
                //Debug.Log("Before or after right");
                right = true;
            }

            // Check Bottom
            if (piece + Vector2.down * blockSpacing == pieceBefore || piece + Vector2.down * blockSpacing == pieceAfter)
            {
                //Debug.Log("Before or after down");
                bottom = true;
            }

            // Check left
            if (piece + Vector2.left * blockSpacing == pieceBefore || piece + Vector2.left * blockSpacing == pieceAfter)
            {
                //Debug.Log("Before or after left");
                left = true;
            }

            // This is so ugly
            // Please improve this at some point

            // Destroy the child blocks
            Destroy(transform.GetChild(i).gameObject);
            GameObject newPiece;

            // Square
            if (top && right && bottom && left)
            {
                //Debug.Log("Normal surrounded block");
                newPiece = Instantiate(GridManager.manager.blockPieces[0], piece, Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right && left)
            {
                //Debug.Log("Normal horizontal block");
                newPiece = Instantiate(GridManager.manager.blockPieces[0], piece, Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (top && bottom)
            {
                //Debug.Log("Normal vertical block");
                newPiece = Instantiate(GridManager.manager.blockPieces[0], piece, Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }

            // Turns
            else if (top && right)
            {
                //Debug.Log("Bottom left turn");
                newPiece = Instantiate(GridManager.manager.blockPieces[1], piece, Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right && bottom)
            {
                //Debug.Log("Top left turn");
                newPiece = Instantiate(GridManager.manager.blockPieces[1], piece, Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (bottom && left)
            {
                //Debug.Log("Top right turn");
                newPiece = Instantiate(GridManager.manager.blockPieces[1], piece, Quaternion.Euler(0, 0, 270f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (left && top)
            {
                //Debug.Log("Bottom right turn");
                newPiece = Instantiate(GridManager.manager.blockPieces[1], piece, Quaternion.Euler(0, 0, 180f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }

            // Top
            else if (bottom)
            {
                //Debug.Log("Upright end piece");
                newPiece = Instantiate(GridManager.manager.blockPieces[2], piece, Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (left)
            {
                //Debug.Log("Right end piece");
                newPiece = Instantiate(GridManager.manager.blockPieces[2], piece, Quaternion.Euler(0, 0, 270f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (top)
            {
                //Debug.Log("bottom end piece");
                newPiece = Instantiate(GridManager.manager.blockPieces[2], piece, Quaternion.Euler(0, 0, 180f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right)
            {
                //Debug.Log("left end piece");
                newPiece = Instantiate(GridManager.manager.blockPieces[2], piece, Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = gameObject.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else
            {
                // Solo block, add a rounded solo block? Circle maybe?
                newPiece = Instantiate(GridManager.manager.blockPieces[0], piece, Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = gameObject.transform;
            }

            if (i == 0)
                head = newPiece;
        }
    }
}
