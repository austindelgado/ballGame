using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthPill : Item
{
    float amount = 0.25f;

    public override void Activate()
    {
        AddBallSize(amount);
    }

    public override void Deactivate()
    {

    }

    public override void Stack()
    {
        Activate();
    }
}
