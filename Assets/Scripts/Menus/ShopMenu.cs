using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopMenu : MonoBehaviour
{
    public List<ItemData> itemList = new List<ItemData>(); // Find a way to autofill this
    // Could have scriptableObjects for each item pool and we grab one of those based off where we are

    public List<ShopSlot> slots = new List<ShopSlot>();

    public void Start()
    {
        // Assign item to each shop slot, these are hardcoded now but should be dynamic later - items that add additional shop slot or something
        for (int i = 0; i < slots.Count; i ++)
        {
            slots[i].AssignItem(itemList[i]);
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(3);
    }
}
