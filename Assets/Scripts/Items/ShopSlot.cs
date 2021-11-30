using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public ItemData itemData;

    public GameObject nameText;
    public GameObject gemText;

    public void Purchase()
    {
        if (GlobalData.Instance.gems >= itemData.gemCost)
        {
            GlobalData.Instance.gems -= itemData.gemCost;
            Debug.Log(itemData.Name + " purchased!");
            GlobalData.Instance.playerInventory.Add(itemData.ID);
        }
        else
            Debug.Log("Not enough gems!");
    }

    public void AssignItem(ItemData item)
    {
        itemData = item;

        nameText.GetComponent<TMP_Text>().text = item.Name;
        gemText.GetComponent<TMP_Text>().text = item.gemCost.ToString();
    }
}
