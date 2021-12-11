using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porcupine : blockObject
{
    void Start()
    {
        gemmed = true;

        float rng = Random.Range(1.25f, 2f);

        // Improve health code at some point please, turn into a single function in the blockObject class
        AreaDatabase AreaDB = Resources.Load<AreaDatabase>("AreaDB");
        depthMod = AreaDB.areas[0].depthMod;
        depth = piecePositions[0].y + GlobalData.Instance.playerDepth;

        // Not sure if depth mod is needed here, proably need to rewrite this
        currentHealth = Mathf.Max(1, Mathf.RoundToInt(depth * depthMod * rng));

        FindLowestY();

        SpawnText();
        Texture();

        Debug.Log("Porcupine spawned!");
    }

    public override void Texture()
    {
        return;
    }

    public override void SpawnText()
    {
        textObject = Instantiate(textPrefab, transform.GetChild(1).position, Quaternion.Euler(0, 0, 0), transform.GetChild(1));
    }
}
