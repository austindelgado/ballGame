using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restock : Item
{
    public override void Activate()
    {
        GlobalData.Instance.restock = true;
    }

    public override void Deactivate()
    {

    }
}
