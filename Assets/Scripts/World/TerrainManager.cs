using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager manager; // Please capitalize

    public float baseSpeed;
    public List<GameObject> bgLayers;

    public bool isMoving;
    Vector2 origPos;
    Vector2 targetPos;

    void Awake()
    {
        if (manager == null)
        {
            // DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    // Descends down by world spacing
    // Add parallax movement alongside this
    public IEnumerator Move(int worldSpacing)
    {
        isMoving = true;

        Coroutine[] coroutines = new Coroutine[bgLayers.Count];
        for (int i = 0; i < bgLayers.Count; i++)
            coroutines[i] = StartCoroutine(bgLayers[i].GetComponent<terrainMover>().Move(worldSpacing, baseSpeed * worldSpacing)); // Add some sort of easing here?
        for (int i = 0; i < coroutines.Length; i++)
            yield return coroutines[i];

        isMoving = false;
    }
}
