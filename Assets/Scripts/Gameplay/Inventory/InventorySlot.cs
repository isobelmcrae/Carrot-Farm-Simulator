using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectedColor, notSelectedColor;

    // deselects all slots on game start
    private void Awake()
    {
        Deselect();
    }

    // changes the colour of the slot if it is selected
    public void Select() {
        image.color = selectedColor;
    }

    // changes the colour of the slot if it is not selected (deselects it)
    public void Deselect() {
        image.color = notSelectedColor;
    }
    
    // if the user drops an item on an empty slot, set the parent of the item to the slot
    public void OnDrop(PointerEventData eventData) {
        if (transform.childCount == 0) {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem inventoryItem = dropped.GetComponent<InventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
        
    }
}
