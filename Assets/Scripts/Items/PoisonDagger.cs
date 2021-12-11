using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : Item
{
    int damage = 1;
    float chance = .05f;
    int multiplier = 2;

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
        if (Random.value < chance + GlobalData.Instance.luck)
        {
            Debug.Log("Poison dagger hit for " + damage);
            block.AddDOT(multiplier * GlobalData.Instance.baseDamage, 3, blockObject.dotType.Bleed);
        }
    }

    public override void Stack()
    {
        chance += 0.05f;
        multiplier++;
    }
}
