using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protractor : Item
{
    public override void Activate()
    {
        GlobalData.Instance.aimIncrease = true;
    }

    public override void Deactivate()
    {

    }
}
