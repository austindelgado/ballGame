using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Item
{
    public override void Activate()
    {
        GameEvents.current.onBlockHit += BonusDamage;
    }

    public override void Deactivate()
    {
        GameEvents.current.onBlockHit -= BonusDamage;
    }

    public void BonusDamage(blockObject block)
    {
        Debug.Log("Dagger hit!");
        block.AddDamage(100);
    }
}
