using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    // This is the slot where the player will click on an item to collect it
    // Should have hover capabilities as well or show description underneath - probably the latter

    // This would be set randomly by the game manager
        // manually set for now
    public Item pickup;

    // References to item slot
    public TMP_Text nameText;
    public Image itemSprite;
    public TMP_Text descriptionText;

    public void Start()
    {
        nameText.text = pickup.name;
        itemSprite = pickup.icon;
        descriptionText.text = pickup.description;
    }

    // Adds the item to the played inventory, intializes it, clears ItemSlot
    // Should be to global data in the future
    public void ItemClicked()
    {
        Debug.Log("Item Selected");

        GlobalData.Instance.GetComponent<GlobalData>().AddItem(pickup);

        // Disable click once equipped
        this.gameObject.GetComponent<Button>().interactable = false;
    }
}
