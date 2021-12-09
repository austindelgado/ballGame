using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRay : Item
{
    float amount = -0.1f;

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
