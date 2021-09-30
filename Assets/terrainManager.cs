using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrainManager : MonoBehaviour
{
    // Turn this into a manager
    public float baseSpeed;
    public List<GameObject> bgLayers;

    bool isMoving;
    Vector2 origPos;
    Vector2 targetPos;

    // Descends down by world spacing
    // Add parallax movement alongside this
    public void Move(int worldSpacing)
    {
        foreach (GameObject layer in bgLayers)
            layer.GetComponent<terrainMover>().Move(worldSpacing, baseSpeed);
    }
}
