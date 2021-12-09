using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemPool", order = 1)]
public class ItemPool : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>();
}
