using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Crit : OnHitEffect
{
    public float chance;
    public int multiplier;

    // NOTE: 
    // Don't actually run the hit effect here, only calculate the damage - this allows for stacking of items

    public override void Activate(GameObject collision)
    {
        if (collision.tag == "Block")
        {
            Debug.Log("Activating " + multiplier + "x Crit with " + chance);

            // Dice roll, if successful, block hit with modifier, else normal hit
            if (Random.value <= chance)
                collision.GetComponent<blockObject>().BlockHit(multiplier * 1);
            else
                collision.GetComponent<blockObject>().BlockHit(1);
        }
    }
}
