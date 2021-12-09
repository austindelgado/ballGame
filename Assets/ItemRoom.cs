using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Should inherit from Room
public class ItemRoom : MonoBehaviour
{
    private List<ItemData> itemList; // Find a way to autofill this
    // Could have scriptableObjects for each item pool and we grab one of those based off where we are
    public ItemPool itemPool;

    public ShopSlot slot;

    public void Start()
    {
        itemList = new List<ItemData>(itemPool.items);

        int toPick = Random.Range(0, itemList.Count);
        slot.AssignItem(itemList[toPick], true);
        itemList.RemoveAt(toPick);
    }
}
