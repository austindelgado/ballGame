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
    public GameObject descText;

    public void Purchase()
    {
        if (GlobalData.Instance.gems >= (int)(itemData.gemCost * (1 - GlobalData.Instance.shopDiscount)))
        {
            GlobalData.Instance.gems -= (int)(itemData.gemCost * (1 - GlobalData.Instance.shopDiscount));
            Debug.Log(itemData.Name + " purchased!");
            GlobalData.Instance.playerInventory.Add(itemData.ID);

            // Disable this gameObject
            HideDescription();

            if (!GlobalData.Instance.restock)
                Destroy(gameObject);
            else
                AssignItem(GameObject.Find("Canvas").GetComponent<ShopMenu>().itemList[Random.Range(0, GameObject.Find("Canvas").GetComponent<ShopMenu>().itemList.Count)]);
        }
        else
            Debug.Log("Not enough gems!");
    }

    public void AssignItem(ItemData item)
    {
        itemData = item;

        nameText.GetComponent<TMP_Text>().text = item.Name;
        gemText.GetComponent<TMP_Text>().text = ((int)(item.gemCost * (1 - GlobalData.Instance.shopDiscount))).ToString();
    }

    public void ShowDescription()
    {
        descText.GetComponent<TMP_Text>().text = itemData.Description;
    }
    
    public void HideDescription()
    {
        descText.GetComponent<TMP_Text>().text = "";
    }
}
