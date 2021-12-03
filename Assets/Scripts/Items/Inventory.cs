using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    Item[] itemList = new Item[10]; // Pull this number from somewhere

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

            case 2:
                newItem = new BallArmor();
                break;

            case 3:
                newItem = new SteamSale();
                break;

            case 4:
                newItem = new BallBundle();
                break;

            case 5:
                newItem = new SingleBall();
                break;

            case 6:
                newItem = new Thorns();
                break;

            case 7:
                newItem = new Protractor();
                break;

            case 8:
                newItem = new Gun();
                break;

            case 9:
                newItem = new TwoLeafClover();
                break;

            case 10:
                newItem = new Restock();
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
