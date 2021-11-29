using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Item
{
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
        Debug.Log("Dagger hit!");
    }
}
