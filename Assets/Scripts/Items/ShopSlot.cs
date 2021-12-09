using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public ItemData itemData;
    public int gemCost;
    public bool isFree;

    public GameObject nameText;
    public GameObject gemText;
    public GameObject descText;

    public void Purchase()
    {
        if (isFree)
        {
            Debug.Log(itemData.Name + " collected!");
            GlobalData.Instance.playerInventory.Add(itemData.ID);
            Destroy(gameObject);
        }
        else if (GlobalData.Instance.gems >= gemCost)
        {

            GlobalData.Instance.gems -= (int)(itemData.gemCost * (1 - GlobalData.Instance.shopDiscount));
            Debug.Log(itemData.Name + " purchased!");
            GlobalData.Instance.playerInventory.Add(itemData.ID);
            
            if (!GlobalData.Instance.restock && itemData.ID != 10)
                Destroy(gameObject);
            else
                AssignItem(GameObject.Find("Shop").GetComponent<ShopMenu>().itemList[Random.Range(0, GameObject.Find("Shop").GetComponent<ShopMenu>().itemList.Count)], false);
        }
        else
            Debug.Log("Not enough gems!");
    }

    public void AssignItem(ItemData item, bool free)
    {
        itemData = item;
        isFree = free;

        nameText.GetComponent<TMP_Text>().text = item.Name;

        if (free)
        {
            gemCost = 0;
            gemText.GetComponent<TMP_Text>().text = "";
        }
        else
        {
            gemCost = (int)(item.gemCost * (1 - GlobalData.Instance.shopDiscount));
            gemText.GetComponent<TMP_Text>().text = gemCost.ToString();   
        }
    }

    public void UpdatePrice()
    {
        gemCost = (int)(gemCost * (1 - GlobalData.Instance.shopDiscount));
        gemText.GetComponent<TMP_Text>().text = gemCost.ToString();
    }

    public void ShowDescription()
    {
        descText.GetComponent<TMP_Text>().text = itemData.Description;
    }
    
    public void HideDescription()
    {
        descText.GetComponent<TMP_Text>().text = "";
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player on interactable!");
            col.gameObject.GetComponent<PlayerMovement>().currentShopSlot = this;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player off interactable!");
            col.gameObject.GetComponent<PlayerMovement>().currentShopSlot = null;
        }
    }
}
