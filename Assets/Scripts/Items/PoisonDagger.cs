using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : Item
{
    int damage = 1;

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
        block.AddDOT(damage, 3, blockObject.dotType.Bleed);
    }

    public override void Stack()
    {
        damage += 2;
    }
}
