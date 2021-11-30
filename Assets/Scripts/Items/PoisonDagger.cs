using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : Item
{
    int damage = 5;

    public override void Activate()
    {
        GameEvents.current.onBlockHit += Poison;
    }

    public override void Deactivate()
    {
        GameEvents.current.onBlockHit -= Poison;
    }

    public void Poison(blockObject block)
    {
        Debug.Log("Poison dagger hit for " + damage);
    }

    public override void Stack()
    {
        damage += 2;
    }
}
