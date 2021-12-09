using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boots : Item
{
    int amount = 2;

    public override void Activate()
    {
        AddMovementSpeed(amount);
    }

    public override void Deactivate()
    {

    }

    public override void Stack()
    {
        Activate();
    }
}
