using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : Item
{
    public override void Activate()
    {
        GlobalData.Instance.split = true;
    }

    public override void Deactivate()
    {

    }
}
