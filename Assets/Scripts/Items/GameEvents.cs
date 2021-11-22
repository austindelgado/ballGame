using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        if (onBlockHit != null)
        {
            //Debug.Log("OnBlockHit activated!");
            onBlockHit();
        }
    }
}