using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class blockObject : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public bool hitThisUpdate = false;

    public GameObject textPrefab;
    public GameObject textObject;

    public float depthMod;
    public float depth;
    private float rng;

    public bool locked;
    public bool gemmed;
    public bool uniqueMove;

    private int bleedDuration;
    private int bleedDamage;

    public Vector2 startingPosition;
    public List<Vector2> piecePositions = new List<Vector2>(); // Update these to include startingPosition?

    public int lowestY;

    [Header("Materials")]
    public Material unlockedMat;
    public Material lockedMat;
    public Material gemUnlockedMat;
    public Material gemLockedMat;

    public enum dotType {Bleed, Poison, Burn};

    // It's a mess in here
        // Rewrite health system and what the value is based off, base depthmod off the level data
        // Rewrite how health is shown on screen, make sure it's optimal
            // Move health changes into blockHit

    void Awake()
    {
        // Grab the textures
        AreaDatabase AreaDB = Resources.Load<AreaDatabase>("AreaDB");
        depthMod = AreaDB.areas[0].depthMod;
        unlockedMat = AreaDB.areas[0].unlocked;
        lockedMat = AreaDB.areas[0].locked;
        gemUnlockedMat = AreaDB.areas[0].gemUnlocked;
        gemLockedMat = AreaDB.areas[0].gemLocked;
    }

    // Start is called before the first frame update
    void Start()
    {
        Texture();
        SpawnText();
        FindLowestY();

        // Figure out formula for weighing this by depth plus some rng
        // OR do this when spawning the blocks in the first place

        // Some starting health * depth modifier * rng
        // Roll for whether this will be a normal, or strong block

        float chance = Random.value;

        if (chance < 0.8f)
            rng = Random.Range(0.8f, 1.2f);
        else
            rng = Random.Range(1.5f, 2f);

        depth = piecePositions[0].y + GlobalData.Instance.playerDepth;

        // Check what this is being based off initially, seems like it's coming from the blockCount?

        // Not sure if depth mod is needed here, proably need to rewrite this
        currentHealth = Mathf.Max(1, Mathf.RoundToInt(depth * depthMod * rng));
    }

    public void FindLowestY()
    {
        int min = 100000;

        for (int i = 0; i < piecePositions.Count; i++)
        {
            if (piecePositions[i].y < min)
                min = (int)piecePositions[i].y;
        }

        lowestY = min;
    }

    void Update()
    {
        // This definitely doesn't need to happen every frame
        if (textObject != null)
            textObject.GetComponent<TMP_Text>().text = currentHealth.ToString();
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
            {
                if (gemmed)
                    piece.gameObject.GetComponent<SpriteRenderer>().material = gemUnlockedMat;
                else
                    piece.gameObject.GetComponent<SpriteRenderer>().material = unlockedMat;
            }
            else if (gemmed)
                piece.gameObject.GetComponent<SpriteRenderer>().material = gemLockedMat;
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

    public void BlockHit()
    {
        // This approach is ugly, find a way to fix the colliders down the road
        // This same thing is being done for ball collisions for scaling
        // It's bad
        //Debug.Log("Block hit for " + damage + " damage");

        AddDamage(1);

        hitThisUpdate = true;
        GameEvents.current.BlockHit(this);
    }

    public void AddDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            BlockBreak();
    }

    public void AddDOT(int damagePerTick, int turnDuration, dotType type)
    {
        if (type == dotType.Bleed)
        {
            // Subscribe to enemyTurnEnd
            if (bleedDuration == 0)
                GameEvents.current.onEnemyTurnEnd += DotTick;

            // Change text color
            textObject.GetComponent<TextMeshPro>().color = Color.red;

            bleedDamage = damagePerTick;
            bleedDuration = turnDuration;
        }
    }

    public void DotTick()
    {
        Debug.Log("BleedDamage: " + bleedDamage + ", bleedDuration: " + bleedDuration);

        if (bleedDuration > 1)
        {
            AddDamage(bleedDamage);
            bleedDuration--;
        }
        else
        {
            AddDamage(bleedDamage);
            bleedDamage = 0;
            GameEvents.current.onEnemyTurnEnd -= DotTick;
            textObject.GetComponent<TextMeshPro>().color = Color.white;
        }
    }

    public void BlockBreak()
    {
        if (bleedDuration >= 0)
            GameEvents.current.onEnemyTurnEnd -= DotTick;

        // If we have gems, add them
        if (gemmed)
            GlobalData.Instance.AddGems((int)(depth / 2));

        GridManager.manager.BlockDestroyed(gameObject);
        Destroy(gameObject);
        GameEvents.current.BlockBreak(this);
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

    public virtual IEnumerator Move()
    {
        Debug.Log("Block move!");
        yield return null;
    }
}
