using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTester : MonoBehaviour
{   
    public TextAsset items;

    // Start is called before the first frame update
    void Start()
    {
        string text = items.text;
        Debug.Log(text);

        ItemData[] itemData = JsonHelper.FromJson<ItemData>(text);
        Debug.Log(itemData[0].Description);
        Debug.Log(itemData[1].Description);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
