using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot_UI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI quantityText;

    // sets the slot to the item in the player's inventory
    public void SetItem(Inventory.Slot slot)
    {
        // checks if slot is not null then sets the item icon, sprite and quantity text
        if(slot != null)
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);
            quantityText.text = slot.count.ToString();
        }
    }

    // sets the slot to empty by setting the item icon sprite to null and the item icon color to transparent, quantity text to empty string
    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);
        quantityText.text = "";
    }
}
