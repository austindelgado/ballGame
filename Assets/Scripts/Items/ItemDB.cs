using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDB : MonoBehaviour
{
    public static List<Item> items = new List<Item> { 
        new Item
        {
            itemID = 1,
            itemName = "Dagger",
            hitEffects = {
                    new Crit { multiplier = 3, chance = .05f },
                }
        },

        new Item
        {
            itemID = 2,
            itemName = "Poison Dagger",
            hitEffects = {
                    new Crit { multiplier = 2, chance = .1f },
                    new Poison { tickDamage = 5, tickRate = 1, duration = 5 }
                }
        }
    };
}
