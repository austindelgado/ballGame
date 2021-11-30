using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;
    public Inventory playerInventory = new Inventory();

    public int seed;

    // Use this to store all player data - Think of it as the save
    // Convert this to a serializer down the line
    [Header("Stats")]
    public float ballsToLaunch;
    public float ballSize; // Maybe a bad idea
    public float aimLength;
    public float luck; // Not active
    public int gems;
    public int playerDepth;

    // World related
    [Header("Area")]
    public float blockHealthStartMod;
    public int areaNum;

    public bool debugMode;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // This doesn't work nicely with the current scene reload
            // Doesn't need to though, focus on setting seed on game start
            // Reason it doesn't work is becasue Awake never gets called again on reload, maybe

        // Set seed
        if (seed == 0) // Generate random
            seed = Random.Range(0, 99999);

        Random.InitState(seed);
    }

    void Start()
    {
        ballsToLaunch = 3;
        gems = 0;
    }

    void Reset()
    {
        // Set seed
        if (seed == 0) // Generate random
            seed = Random.Range(0, 99999);

        Random.InitState(seed);

        ballsToLaunch = 3;
        gems = 0;
    }

    // I don't like player specific stuff being here, but GlobalData maybe shouldn't exist at all anyways
    public void AddGems(int numGems)
    {
        gems += numGems;
    }
}
