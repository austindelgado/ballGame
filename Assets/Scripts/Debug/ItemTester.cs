using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{   
    Inventory playerInventory = new Inventory();
    public int id = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory.Add(id);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
