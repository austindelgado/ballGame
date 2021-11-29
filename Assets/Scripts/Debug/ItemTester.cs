using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{   
    public void OnBlockHit()
    {
        GameEvents.current.BlockHit(null);
    }
}
