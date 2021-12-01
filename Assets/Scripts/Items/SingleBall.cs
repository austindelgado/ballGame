using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBall : Item
{
    public override void Activate()
    {
        AddBalls(1);
    }

    public override void Deactivate()
    {

    }
    
    public override void Stack()
    {
        Activate();
    }
}
