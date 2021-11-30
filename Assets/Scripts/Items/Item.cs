using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Hi bella!

// hi bubba! ;*

[System.Serializable]
public abstract class Item
{
    public abstract void Activate(); // Stick GameEvents you need to subscribe to in here, this needs to be called by wherever the item is added.
    // This should also handle any item synergies that need to be hardcoded

    public abstract void Deactivate(); // Called when an item is removed, needs to remove event subscriptions

    public virtual void Stack() // Used to implement custom stacking for items
    {
        Debug.Log("No stacking implemented");
    }
}
