using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    // default selected slot is -1 (none as there is no 0th slot)
    int selectedSlot = -1;

    // changes the selected slot to the given value (0-7)
    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }

    // if the user inputs a number from 1-7, change the selected slot to that num - 1
    private void Update() {
        if (Input.inputString !=null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 8)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    // adds an item to the inventory
    public bool AddItem(Item item) {

        // stackable items
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < 99) {
                itemInSlot.count++;
                itemInSlot.UpdateCount();
                return true;
            }
        }

        // checks for inventory spaces
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    // adds new item to the inventory slot
    void SpawnNewItem(Item item, InventorySlot slot) {
        GameObject newItem = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    // gets currently selected item, and if use is true, removes one of it from the inventory
    public Item GetSelectedItem(bool use) {
        InventorySlot slot = inventorySlots[selectedSlot];
        if (selectedSlot != -1) {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) {
            Item item = itemInSlot.item;
            if (use == true)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.UpdateCount();
                }
            }
            return item;
            } else return null;
        }
        return null;
        
    }   

    DontDestroyOnLoad(this.gameObject);
}
