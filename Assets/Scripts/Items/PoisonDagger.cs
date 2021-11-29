using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDagger : Item
{
    int damage = 5;

    public override void Activate()
    {
        GameEvents.current.onBlockHit += Trigger;
    }

    public override void Deactivate()
    {
        GameEvents.current.onBlockHit -= Trigger;
    }

    public override void Trigger()
    {
        Debug.Log("Poison dagger hit for " + damage);
    }

    public override void Stack()
    {
        damage += 2;
    }
}
