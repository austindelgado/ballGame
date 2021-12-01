using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallArmor : Item
{
    public override void Activate()
    {
        AddBallHealth(1);
    }

    public override void Deactivate()
    {

    }
}
