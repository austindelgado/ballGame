using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : HitEffect
{
    public int tickDamage;
    public int tickRate;
    public int duration;

    public override void Hit()
    {
        Debug.Log("Applying poison that deals " + tickDamage + " every " + tickRate + " turns for " + duration + " turns!");
    }
}
