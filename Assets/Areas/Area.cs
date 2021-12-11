using UnityEngine;

[CreateAssetMenu(fileName = "Area", menuName = "ScriptableObjects/Area")]
public class Area : ScriptableObject
{
    public int areaID;

    // Sprite reference
    [Header("Sprites")]
    public Material unlocked;
    public Material locked;
    public Material gemUnlocked;
    public Material gemLocked;
    public Texture bg;

    [Header("Level Generation")]
    public int depth;
    public float depthMod;
    public int maxBlockSize;
    public int minBlockSize;
    public float deletePercentage;
    public float gemChance;
    public int chanceUp;
    public int chanceRight;
    public int chanceDown;
    public int chanceLeft;

    [Header("Enemies")]
    public GameObject[] enemies; // Change to value key pair for weight
    public int minEnemies;
    public int maxEnemies;
}
