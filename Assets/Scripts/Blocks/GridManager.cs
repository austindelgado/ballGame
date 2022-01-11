using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

// Change to be similar to other Manager start

public class GridManager : MonoBehaviour
{
    public static GridManager manager;

    [Header("Current Level")]
    public Area currentArea;
    public int playerDepth = 0;

    [Header("Prefabs")]
    public GameObject blockEmpty;
    public GameObject piece;
    public GameObject numScoreText;

    [Header("Positioning and Size")]
    public int startingX; // Remove
    public int startingY; // Remove
    public int startingBuffer;
    public int enemySpawnBuffer;
    public int maxDepthDiff;
    public int losingHeight;

    public int score = -1;

    public int rows;
    public int cols;
    public int blockSpacing = 1;

    public GameObject[,] grid;
    private int numBlocks;
    public List<GameObject> blocks = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public List<Room> rooms = new List<Room>();

    [Header("Generation Variables")]
    public int maxBlockSize;
    public int minBlockSize;
    public float deletePercentage;

    [Header("Block Pieces")]
    public GameObject[] blockPieces; // Piece prefabs

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Do more research into loading in general
        // Grab current level scriptableObject
            // Change this to just being on the GridManager?
        AreaDatabase AreaDB = Resources.Load<AreaDatabase>("AreaDB");
        currentArea = AreaDB.areas[GlobalData.Instance.areaNum];

        startingX = currentArea.startingX;
        startingY = currentArea.startingY;

        // Rows is decided based off level size
        rows = currentArea.rows;
        cols = currentArea.cols;
        
        // Use this for inital spawn
        grid = new GameObject[cols, rows + 1];
        // Rows = y
        for (int y = 0; y < rows + 1; y++)
        {
            // Columns = x
            for (int x = 0; x < cols; x++)
            {
                grid[x, y] = null;
            }
        }

        SpawnBlocks(currentArea.startingBuffer);
        SpawnEnemies(currentArea.startingBuffer + currentArea.enemySpawnBuffer);
        RemoveBlocks();
        //MoveBlocks(true);
        CheckAllGravity(true);
        CheckAllGravity(true); // Still random floating blocks for some reason
    }

    public void SpawnBlocks(int buffer)
    {
        for (int y = buffer; y < rows; y++)
        {
            // Columns = x
            for (int x = 0; x < cols; x++)
            {
                if (grid[x, y] == null)
                    SpawnBlock(x, y);
            }
        }
    }

    // Please move score update and scene reset out of here at some point
        // Give each block object the responsibility of moving?
    public IEnumerator MoveBlocks(bool start) // Grid position does not change with this call, moves ALL blocks
    {
        if (blocks.Count == 0 && enemies.Count == 0)
        {
            // Store playerDepth
            GlobalData.Instance.playerDepth += playerDepth;

            StartCoroutine(GameManager.manager.LevelWon());
            yield break;
        }

        if (!start)
        { 
            //numScoreText.GetComponent<TMP_Text>().text = GlobalData.Instance.playerDepth.ToString();

            // Advance player down a level

            // Actually move the level
            // Check if first block is above the minimum y
            // Player Depth + maxDepthDiff gives min block height
            int depthDiff = (int)blocks[0].GetComponent<blockObject>().lowestY - playerDepth - maxDepthDiff;

            if (enemies.Count != 0)
                depthDiff = Math.Min(depthDiff, (int)enemies[0].GetComponent<blockObject>().lowestY - playerDepth - maxDepthDiff);

            if (rooms.Count != 0)
                depthDiff = Math.Min(depthDiff, rooms[0].roomDepth - playerDepth);
                
            if (depthDiff > 0)
            {
                playerDepth += depthDiff;
                yield return StartCoroutine(TerrainManager.manager.Move(blockSpacing * (int)depthDiff));
            }
            else
            {
                playerDepth++;
                yield return StartCoroutine(TerrainManager.manager.Move(blockSpacing));
            }
        }

        // Write GetLowestBlock and GetLowestEnemy to avoid errors here
        if (GetLowestBlock() == playerDepth + 1 || GetLowestEnemy() == playerDepth + 1) // Block here!
        {
            StartCoroutine(GameManager.manager.LevelLost());
            yield break;
        }

        yield return null;
    }

    private int GetLowestBlock()
    {
        if (blocks.Count != 0)
            return (int)blocks[0].GetComponent<blockObject>().lowestY;
        else
            return 100000000; // Change this
    }

    private int GetLowestEnemy()
    {
        if (enemies.Count != 0)
            return (int)enemies[0].GetComponent<blockObject>().lowestY;
        else
            return 100000000;
    }

    public IEnumerator MoveEnemies()
    {
        Coroutine[] coroutines = new Coroutine[enemies.Count];
        for (int i = 0; i < enemies.Count; i++)
            coroutines[i] = StartCoroutine(enemies[i].GetComponent<blockObject>().Move());
        for (int i = 0; i < coroutines.Length; i++)
            yield return coroutines[i];
    }

    private void SpawnBlock(int x, int y)
    {
        Vector2 startingPos = new Vector2(startingX + (blockSpacing * x), startingY + (blockSpacing * y));

        // Creates parent block
        GameObject newBlock = Instantiate(blockEmpty, new Vector2(startingX + (blockSpacing * x), startingY - (blockSpacing * y)), Quaternion.Euler(0, 0, 0));
        newBlock.transform.parent = gameObject.transform.GetChild(0);
        newBlock.name = "Block " + (blocks.Count + 1);
        newBlock.GetComponent<blockObject>().currentHealth = blocks.Count + 1;
        blocks.Add(newBlock);

        List<Vector2> piecePos = new List<Vector2>();

        // Inital piece
        grid[x, y] = newBlock;
        piecePos.Add(new Vector2(x, y));

        // Additional pieces
        for (int i = 0; i < currentArea.maxBlockSize - 1; i++)
        {
            //List<Vector2> possibleMoves = new List<Vector2>();
            List<int> possibleMoves = new List<int>();
            List<Vector2> possibleUp = new List<Vector2>();
            List<Vector2> possibleRight = new List<Vector2>();
            List<Vector2> possibleDown = new List<Vector2>();
            List<Vector2> possibleLeft = new List<Vector2>();

            // 0 - Up
            // 1 - Right
            // 2 - Down
            // 3 - Left

            // Finds all possible moves for all pieces
            foreach (Vector2 piece in piecePos)
            {
                // Check top
                if (piece.y > startingBuffer && piece.y - 1 > -1 && grid[(int)piece.x, (int)piece.y - 1] == null)
                {
                    possibleUp.Add(new Vector2(piece.x, piece.y - 1));
                }

                // Check right
                if (piece.x + 1 < cols && grid[(int)piece.x + 1, (int)piece.y] == null)
                {
                    possibleRight.Add(new Vector2(piece.x + 1, piece.y));
                }

                // Check bottom
                if (piece.y + 1 < rows && grid[(int)piece.x, (int)piece.y + 1] == null)
                {
                    possibleDown.Add(new Vector2(piece.x, piece.y + 1));
                }

                // Check left
                if (piece.x - 1 > -1 && grid[(int)piece.x - 1, (int)piece.y] == null)
                {
                    possibleLeft.Add(new Vector2(piece.x - 1, piece.y));
                }
            }

            // Throw possible move into list = weight 
            if (possibleUp.Count != 0)
            {
                for (int j = 0; j < currentArea.chanceUp; j++)
                    possibleMoves.Add(0);
            }

            if (possibleRight.Count != 0)
            {
                for (int j = 0; j < currentArea.chanceRight; j++)
                    possibleMoves.Add(1);
            }

            if (possibleDown.Count != 0)
            {
                for (int j = 0; j < currentArea.chanceDown; j++)
                    possibleMoves.Add(2);
            }

            if (possibleLeft.Count != 0)
            {
                for (int j = 0; j < currentArea.chanceLeft; j++)
                    possibleMoves.Add(3);
            }

            // Pick random possible moves
            if (possibleMoves.Count != 0)
            {
                // Select random move direction from the list
                int selectedDirection = possibleMoves[UnityEngine.Random.Range(0, possibleMoves.Count)];

                // Change this to weigh directions
                Vector2 nextMove;

                if (selectedDirection == 0)
                    nextMove = possibleUp[UnityEngine.Random.Range(0, possibleUp.Count)];
                else if (selectedDirection == 1)
                    nextMove = possibleRight[UnityEngine.Random.Range(0, possibleRight.Count)];
                else if (selectedDirection == 2)
                    nextMove = possibleDown[UnityEngine.Random.Range(0, possibleDown.Count)];
                else if (selectedDirection == 3) // SelectedDirection == 3
                    nextMove = possibleLeft[UnityEngine.Random.Range(0, possibleLeft.Count)];
                else
                    return;

                grid[(int)nextMove.x, (int)nextMove.y] = newBlock;
                piecePos.Add(nextMove);
            }
            else
            {
                // Minimum 2 pieces for a block
                if (piecePos.Count < currentArea.minBlockSize)
                {
                    Remove(newBlock, true);
                    return;
                }
            }
        }


        // This code is ugly but it works, don't think it's the problem
        // Organizing pieces should be the problem of the actual blockObject maybe
        // Organize corner pieces
        foreach (Vector2 piece in piecePos)
        {
            // Assign these pieces to the block
            newBlock.GetComponent<blockObject>().piecePositions = piecePos;

            // True if a block is found
            bool top = false;
            bool right = false;
            bool bottom = false;
            bool left = false;

            // Check Top
            if (piece.y - 1 > -1 && grid[(int)piece.x, (int)piece.y - 1] == newBlock)
            {
                top = true;
            }

            // Check Right
            if (piece.x + 1 < cols && grid[(int)piece.x + 1, (int)piece.y] == newBlock)
            {
                right = true;
            }

            // Check Bottom
            if (piece.y + 1 < rows && grid[(int)piece.x, (int)piece.y + 1] == newBlock)
            {
                bottom = true;
            }

            // Check left
            if (piece.x - 1 > -1 && grid[(int)piece.x - 1, (int)piece.y] == newBlock)
            {
                left = true;
            }

            // This is so ugly
            // Please improve this at some point

            // Square
            if (top && right && bottom && left)
            {
                //Debug.Log("Normal surrounded block");
                GameObject newPiece = Instantiate(blockPieces[0], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right && left)
            {
                //Debug.Log("Normal horizontal block");
                GameObject newPiece = Instantiate(blockPieces[0], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (top && bottom)
            {
                //Debug.Log("Normal vertical block");
                GameObject newPiece = Instantiate(blockPieces[0], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }

            // Turns
            else if (top && right)
            {
                //Debug.Log("Bottom left turn");
                GameObject newPiece = Instantiate(blockPieces[1], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right && bottom)
            {
                //Debug.Log("Top left turn");
                GameObject newPiece = Instantiate(blockPieces[1], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (bottom && left)
            {
                //Debug.Log("Top right turn");
                GameObject newPiece = Instantiate(blockPieces[1], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 270f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (left && top)
            {
                //Debug.Log("Bottom right turn");
                GameObject newPiece = Instantiate(blockPieces[1], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 180f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }

            // Top
            else if (bottom)
            {
                //Debug.Log("Upright end piece");
                GameObject newPiece = Instantiate(blockPieces[2], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 0));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (left)
            {
                //Debug.Log("Right end piece");
                GameObject newPiece = Instantiate(blockPieces[2], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 270f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (top)
            {
                //Debug.Log("bottom end piece");
                GameObject newPiece = Instantiate(blockPieces[2], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 180f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else if (right)
            {
                //Debug.Log("left end piece");
                GameObject newPiece = Instantiate(blockPieces[2], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = newBlock.transform;
                // newPiece.GetComponent<SpriteRenderer>().color = blockColor;
            }
            else
            {
                // Solo block, add a rounded solo block? Circle maybe?
                GameObject newPiece = Instantiate(blockPieces[0], new Vector2(startingX + piece.x * blockSpacing, startingY - piece.y * blockSpacing), Quaternion.Euler(0, 0, 90f));
                newPiece.transform.parent = newBlock.transform;
            }
        }

        // This block is staying, give it a chance to have gems
        if (UnityEngine.Random.value <= currentArea.gemChance + GlobalData.Instance.luck)
            newBlock.GetComponent<blockObject>().gemmed = true;
    }

    private void SpawnEnemies(int buffer)
    {
        // Decide number of enemies that we want in the level, base off a level stat?
        // Select from possible enemies, weigh these for tougher enemies
        // Create the enemy object, call spawn(), have this return true if spawn successful
        // Otherwise, try a different position
        // Only spawn enemies below a certain threshold, shouldn't see them from starting screen

        // Things break when you do 1,1, no clue why
        int toSpawn = UnityEngine.Random.Range(currentArea.minEnemies, currentArea.maxEnemies);

        // First attempt at some pseudorandom

        float enemySpawnChance = 0.1f;
        float currentEnemySpawnChance = enemySpawnChance;

        for (int y = buffer; y < rows; y++)
        {
            if (toSpawn > 0 && UnityEngine.Random.value < currentEnemySpawnChance)
            {
                SpawnEnemy(UnityEngine.Random.Range(0,4), y);
                toSpawn--;
                y += 3;
                currentEnemySpawnChance = enemySpawnChance; 
            }
            else if(toSpawn > 0)
                currentEnemySpawnChance += 0.1f;
        }
    }

    private void SpawnEnemy(int x, int y)
    {
        // Creates parent block
        int enemyPick = UnityEngine.Random.Range(0, currentArea.enemies.Length);
        GameObject newEnemy = Instantiate(currentArea.enemies[enemyPick], new Vector2(startingX + (blockSpacing * x), startingY - (blockSpacing * y)), Quaternion.Euler(0, 0, 0));
        newEnemy.transform.parent = gameObject.transform.GetChild(1);
        newEnemy.name = currentArea.enemies[enemyPick].name;

        // If we make it here, we're spawning this enemy
        // Go over all pieces, delete any blocks there

        // If there is a block here, delete it
        Debug.Log("Adding enemy " + newEnemy.name);

        if (grid[x, y] != null)
            Remove(grid[x, y], true);

        enemies.Add(newEnemy);

        // Consider moving this into the block
        //foreach (Vector2 piece in newEnemy.GetComponent<blockObject>().piecePositions)
        for (int i = 0; i < newEnemy.GetComponent<blockObject>().piecePositions.Count; i++)
        {
            Vector2 piece = newEnemy.GetComponent<blockObject>().piecePositions[i];

            if (grid[(int)piece.x + x, (int)piece.y + y] != null)
                Remove(grid[(int)piece.x + x, (int)piece.y + y], true);

            grid[(int)piece.x + x, (int)piece.y + y] = newEnemy;
            newEnemy.GetComponent<blockObject>().piecePositions[i] = new Vector2((int)piece.x + x, (int)piece.y + y);
        }

        // Store starting position value
        newEnemy.GetComponent<blockObject>().startingPosition = new Vector2(x, y);
    }

    private void RemoveBlocks()
    {
        // Find about how many to remove for percentage 
        int toRemove = Mathf.CeilToInt(blocks.Count * currentArea.deletePercentage);

        // Bagged Random?

        for (int i = 0; i < toRemove; i++)
        {
            // Get a random block
            GameObject currentBlock = blocks[UnityEngine.Random.Range(0, blocks.Count)];
            if (currentBlock.tag == "Block")
                Remove(currentBlock, true);
            else
                toRemove++;
        }
    }

    // Removes specific block from the grid/list
    public void Remove(GameObject block, bool gridDestroy)
    {
        // Check if this is also an enemy
        if (block.tag == "Enemy")
            enemies.Remove(block);
        else
            blocks.Remove(block); // Remove from list

        foreach (Vector2 piecePos in block.GetComponent<blockObject>().piecePositions)
        {
            grid[(int)piecePos.x, (int)piecePos.y] = null;
        }

        if (gridDestroy)
            Destroy(block);
        else
            CheckAllGravity(false);
    }

    // This is called for playerDestroyed blocks
    public void BlockDestroyed(GameObject block)
    {
        Debug.Log("GridMananger: Block Destroyed");
        Remove(block, false);
    }

    // Change this bool to an override or something? IDK
    public void CheckAllGravity(bool gridSpawn)
    {
        if (blocks.Count == 0)
            return;

        //foreach (GameObject block in blocks)
        for (int i = blocks.Count - 1; i >= 0;  i--)
        {
            if (blocks[i] == null)
                break;

            GameObject block = blocks[i];

            // Assume not supported until we start locking blocks
            bool supported = false;

            // Don't bother checking if it's locked
            if (block.GetComponent<blockObject>().locked)
                supported = true;
            else
            {
                foreach (Vector2 piecePos in block.GetComponent<blockObject>().piecePositions)
                {
                    // Find single block of support
                    if (grid[(int)piecePos.x, (int)piecePos.y + 1] != null && grid[(int)piecePos.x, (int)piecePos.y + 1].tag == "Block" && grid[(int)piecePos.x, (int)piecePos.y] != grid[(int)piecePos.x, (int)piecePos.y + 1])
                    {
                        // check if supported by higher block
                        if (blocks.IndexOf(grid[(int)piecePos.x, (int)piecePos.y + 1]) < blocks.IndexOf(grid[(int)piecePos.x, (int)piecePos.y]))
                        {
                            //Debug.Log(block.name + " is supported by higher block, checking " + grid[(int)piecePos.x, (int)piecePos.y + 1].name);
                            if (!CheckBlockGravity(grid[(int)piecePos.x, (int)piecePos.y + 1], gridSpawn)) // I don't like this but need to jump two places when iterating
                                i--;
                        }
                        else
                        {
                            supported = true;
                        }
                    }
                }
            }

            if (!supported) // Not supported
            {
                // Lock
                if (gridSpawn)
                {
                    block.GetComponent<blockObject>().Lock();
                }
                else // Destroy
                {
                    block.GetComponent<blockObject>().BlockBreak(); // Player destroyed
                    //Debug.Log(block.name + " is not supported");
                }
            }
        }   
    }

    public bool CheckBlockGravity(GameObject block, bool gridSpawn)
    {
        // Don't bother checking if it's locked
        if (block.GetComponent<blockObject>().locked)
            return true;

        // Assume not supported until we start locking blocks
        bool supported = false;

        foreach (Vector2 piecePos in block.GetComponent<blockObject>().piecePositions)
        {
            if ((int)piecePos.y + 1 >= rows || grid[(int)piecePos.x, (int)piecePos.y + 1] == null || grid[(int)piecePos.x, (int)piecePos.y + 1].tag != "Block") // Gotta deal with bottom row blocks
                break;

            // Find single block of support
            if (grid[(int)piecePos.x, (int)piecePos.y] != grid[(int)piecePos.x, (int)piecePos.y + 1])
                supported = true;
        }

        if (!supported)
        {
            if (gridSpawn)
                Remove(block, true);
            else
                block.GetComponent<blockObject>().BlockBreak(); // Player destroyed
            //Debug.Log(block.name + " is NOT supported by after check");
            return false;
        }
        else
        {
            //Debug.Log(block.name + " is supported after check");
            return true;
        }
    }
}
