using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crit : HitEffect
{
    public float multiplier;
    public float chance;

    public override void Hit()
    {
        Debug.Log("Crit for " + multiplier + "x damage with " + chance * 100 +"% chance!");
    }
}
