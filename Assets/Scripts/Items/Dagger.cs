using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Item
{
    public float critChance = 0.05f;
    public int multiplier = 1;

    public override void Activate()
    {
        GameEvents.current.onBlockHit += Crit;
    }

    public override void Deactivate()
    {
        GameEvents.current.onBlockHit -= Crit;
    }

    public void Crit(blockObject block)
    {
        if (Random.value < critChance + GlobalData.Instance.luck)
        {
            Debug.Log("Dagger crit!");
            block.AddDamage(multiplier * GlobalData.Instance.baseDamage);
        }
    }

    public override void Stack()
    {
        critChance += 0.05f;
        multiplier++;
    }
}
