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

    public void AddBallHealth(int amount)
    {
        GlobalData.Instance.ballStartingHealth += amount;
    }

    public void AddBalls(int amount)
    {
        GlobalData.Instance.ballsToLaunch += amount;
    }

    public void AddBaseDamage(int amount)
    {
        GlobalData.Instance.baseDamage += amount;
    }

    public void AddLuck(float amount)
    {
        GlobalData.Instance.luck += amount;
    }

    public void AddBallSize(float amount)
    {
        GlobalData.Instance.ballSize += amount;
        if (GlobalData.Instance.ballSize < 0.25f)
            GlobalData.Instance.ballSize = 0.25f;
        else if (GlobalData.Instance.ballSize > 2f)
            GlobalData.Instance.ballSize = 2f;
    }

    public void AddMovementSpeed(int amount)
    {
        GlobalData.Instance.playerSpeed += amount;
    }

    public void AddAttackCD(float amount)
    {
        GlobalData.Instance.attackCD += amount;
    }
}
