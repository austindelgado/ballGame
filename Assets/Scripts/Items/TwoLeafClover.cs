using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoLeafClover : Item
{
    float luck = 0.05f;

    public override void Activate()
    {
        AddLuck(luck);
    }

    public override void Deactivate()
    {

    }
}
