using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public override void Activate()
    {
        AddBaseDamage(3);
    }

    public override void Deactivate()
    {

    }
}
