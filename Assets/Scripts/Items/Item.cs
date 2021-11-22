using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Item
{
    public int itemID;
    public string itemName;

    public List<HitEffect> hitEffects = new List<HitEffect>();

    public void Equip()
    {
        Debug.Log(itemName + " equipped!");

        // I don't think I love it, but this is where items check for other items when being equipped

        bool addAll = true; // Set false to not include the added item and hitEffects

        if (itemID == 1) // Synergies for ID: 1 - Dagger 
        {
            if (GlobalData.Instance.itemList.Contains(ItemDB.items[0])) // This is how you check for the specific item
            {
                // Doing this to access data in the subclass seems very messy but it works

                // Added Dagger, add +1 to multipler and double chance, this part could in theory be the individual classes problem, have a Stack()
                ((Crit)hitEffects[0]).multiplier += 1;
                ((Crit)hitEffects[0]).chance += 0.05f;
                addAll = false;
            }
        }

        if (addAll)
        {
            ballLauncher bLauncher = GameObject.Find("Ball Launcher").GetComponent<ballLauncher>();
            GlobalData.Instance.itemList.Add(this);
            foreach (HitEffect hitEffect in hitEffects)
                bLauncher.defaultHitEffects.Add(hitEffect);
        }
    }
}
