using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBundle : Item
{
    public override void Activate()
    {
        AddBalls(3);
    }

    public override void Deactivate()
    {

    }

    public override void Stack()
    {
        Activate();
    }
}
