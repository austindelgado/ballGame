using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ItemDB.items[0].Equip();
        //Debug.Log(GlobalData.Instance.itemList[0].itemName);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ItemDB.items[1].Equip();
        }
    }
}
