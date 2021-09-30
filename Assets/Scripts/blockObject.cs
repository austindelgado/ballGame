using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blockObject : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public bool hitThisUpdate = false;

    public int blockSize;

    public GameObject textPrefab;
    public GameObject textObject;

    public float depthMod;
    public float depth;
    private float rng;

    public bool locked;
    public bool uniqueMove;

    public Vector2 startingPosition;
    public List<Vector2> piecePositions = new List<Vector2>(); // Update these to include startingPosition?

    [Header("Materials")]
    public Material unlockedMat;
    public Material lockedMat;


    // It's a mess in here
        // Rewrite health system and what the value is based off, base depthmod off the level data
        // Rewrite how health is shown on screen, make sure it's optimal
            // Move health changes into blockHit

    void Awake()
    {
        // Grab the textures
        AreaDatabase AreaDB = Resources.Load<AreaDatabase>("AreaDB");
        unlockedMat = AreaDB.areas[0].unlocked;
        lockedMat = AreaDB.areas[0].locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        Texture();
        SpawnText();

        // Figure out formula for weighing this by depth plus some rng
        // OR do this when spawning the blocks in the first place

        // Some starting health * depth modifier * rng
        // Roll for whether this will be a normal, or strong block

        float chance = Random.value;

        if (chance < 0.8f)
        {
            rng = Random.Range(0.8f, 1.2f);
            blockSize = 0;
        }
        else
        {
            rng = Random.Range(1.5f, 2f);
            blockSize = 1;
        }

        depth = -transform.position.y;

        // Check what this is being based off initially, seems like it's coming from the blockCount?

        // Not sure if depth mod is needed here, proably need to rewrite this
        currentHealth = Mathf.Max(1, Mathf.RoundToInt(((depth + GlobalData.Instance.blockHealthStartMod) * depthMod) * startingHealth * rng));
    }

    void Update()
    {
        // This definitely doesn't need to happen every frame
        if (textObject != null)
            textObject.GetComponent<TMP_Text>().text = currentHealth.ToString();

        // This either
        if (currentHealth <= 0)
        {
            GridManager.manager.BlockDestroyed(gameObject);
            //Destroy(gameObject);
            GameManager.manager.BlockBreak(); // Let GameManager know a block broke
        }
    }

    void FixedUpdate()
    {
        hitThisUpdate = false;
    }

    public virtual void Texture()
    {
        // Assign them
        foreach (Transform piece in transform)
        {
            if (!piece.GetComponent<pieceObject>().lockCheck)
                piece.gameObject.GetComponent<SpriteRenderer>().material = unlockedMat;
        }
    }

    public virtual void SpawnText()
    {
        // Find the child with the highest y value
        Transform highestChild = null;

        foreach (Transform child in transform)
        {
            if (highestChild == null)
                highestChild = child;
            else if (child.position.y > highestChild.position.y)
                highestChild = child;
        }

        // Spawn text at its position
        textObject = Instantiate(textPrefab, highestChild.position, Quaternion.Euler(0, 0, 0), gameObject.transform);
    }

    public void BlockHit(int damage)
    {
        // This approach is ugly, find a way to fix the colliders down the road
        // This same thing is being done for ball collisions for scaling
        // It's bad
        //Debug.Log("Block hit for " + damage + " damage");

        hitThisUpdate = true;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GridManager.manager.BlockDestroyed(gameObject);
            //Destroy(gameObject);
            GameManager.manager.BlockBreak(); // Let GameManager know a block broke
        }
    }

    public void Lock()
    {
        // Loop through pieces
        // Change texture
        locked = true;

        foreach (Transform piece in transform)
        {
            if (piece.gameObject.tag == "Block")
            {
                piece.gameObject.GetComponent<SpriteRenderer>().material = lockedMat;
                piece.gameObject.GetComponent<pieceObject>().lockCheck = true;
            }
        }
    }


    public virtual void Move()
    {
        Debug.Log("Block move!");
    }
}
