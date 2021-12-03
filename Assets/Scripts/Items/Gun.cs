using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Item
{
    public override void Activate()
    {
        AddBaseDamage(2);
    }

    public override void Deactivate()
    {

    }

    public override void Stack()
    {
        Activate();
    }
}
