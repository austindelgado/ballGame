using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWorm : MonoBehaviour
{
    public TransparencyToggle outer;
    private bool playerIn = false;
    public int depth;

    // Update is called once per frame
    void Update()
    {
        if (depth <= GridManager.manager.playerDepth && !playerIn)
        {
            playerIn = true;
            outer.Toggle();
        }
    }
}
