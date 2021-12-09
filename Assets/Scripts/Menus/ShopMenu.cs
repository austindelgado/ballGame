using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public List<ItemData> itemList = new List<ItemData>(); // Find a way to autofill this
    // Could have scriptableObjects for each item pool and we grab one of those based off where we are
    public ItemPool itemPool;

    public List<ShopSlot> slots = new List<ShopSlot>();

    public void Start()
    {
        itemList = new List<ItemData>(itemPool.items);

        // Assign item to each shop slot, these are hardcoded now but should be dynamic later - items that add additional shop slot or something
        for (int i = 0; i < slots.Count; i ++)
        {
            int toPick = Random.Range(0, itemList.Count);
            slots[i].AssignItem(itemList[toPick], false);
            itemList.RemoveAt(toPick);
        }
    }

    public void UpdatePrices()
    {
        for (int i = 0; i < slots.Count; i ++)
            slots[i].UpdatePrice();
    }
}
