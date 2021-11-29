using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    Item[] itemList = new Item[2]; // Pull this number from somewhere

    // This needs a permanent home
    public void Add(int itemID)
    {
        Item newItem = null;

        // Stacking works well
        if (itemList[itemID] != null)
        {
            itemList[itemID].Stack();
            return;
        }

        // This is where the actual assigning happens
        // Might get massive, not sure how to make it better
        switch (itemID)
        {
            case 0: 
                newItem = new Dagger();
                break;

            case 1:
                newItem = new PoisonDagger();
                break;

            default:
                Debug.Log("Item ID mismatch");
                break;
        }

        // Check we got an item match before activating and adding to itemList
        if (newItem != null) 
        {
            newItem.Activate();
            itemList[itemID] = newItem;
        }
    }
}
