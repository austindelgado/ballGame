using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// All possible game events should go in here, follow BlockHit() as a template
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onBlockHit;
    public void BlockHit()
    {
        Debug.Log("OnBlockHit activated!");
        
        if (onBlockHit != null)
        {
            onBlockHit();
        }
    }
}