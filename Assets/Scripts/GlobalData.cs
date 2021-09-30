using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    public int seed;

    // Use this to store all player data - Think of it as the save
    // Convert this to a serializer down the line
    [Header("Items")]
    public List<Item> itemList = new List<Item>();

    [Header("Stats")]
    public float ballsToLaunch;
    public float ballSize; // Maybe a bad idea
    public float aimLength;
    public float luck; // Not active

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

    void Reset()
    {
        // Set seed
        if (seed == 0) // Generate random
            seed = Random.Range(0, 99999);

        Random.InitState(seed);
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        item.Initialize();
    }
}
