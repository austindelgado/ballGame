using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// All OnHitEffects must take in the hit block
public abstract class OnHitEffect : ScriptableObject
{
    public virtual void Activate(GameObject block)
    {
        Debug.Log("Default activate");
    }
}