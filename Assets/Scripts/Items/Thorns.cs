using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : Item
{
    public override void Activate()
    {
        AddBaseDamage(1);
    }

    public override void Deactivate()
    {

    }
}
