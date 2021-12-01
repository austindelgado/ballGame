using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamSale : Item
{
    public override void Activate()
    {
        GlobalData.Instance.shopDiscount = 0.2f; // 15 percent discount
    }

    public override void Deactivate()
    {

    }
}
