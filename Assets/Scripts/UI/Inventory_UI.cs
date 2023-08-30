using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Player player;

    // list of slots in inventory
    public List<Slot_UI> slots = new List<Slot_UI>();

    // Update is called once per frame
    void Update()
    {
        // if tab is pressed, toggle inventory
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        // if inventory is not active, set it to active and refresh inventory, else set it to inactive
        if(!inventoryPanel.activeSelf)
        {
            inventoryPanel.SetActive(true);
            Refresh();
        } else {
            inventoryPanel.SetActive(false);
        }
    }


    void Refresh()
    {
        // if the number of slots in the inventory UI is the same as the number of slots in the player's inventory
        if(slots.Count == player.inventory.slots.Count)
        {
            // for each slot in the inventory UI
            for(int i =0; i < slots.Count; i++)
            {
                // if the player's inventory slot is not empty, set the inventory UI slot to the player's inventory slot
                if(player.inventory.slots[i].type != CollectableType.NONE)
                {
                    // sets the slot to the item in the player's inventory
                    slots[i].SetItem(player.inventory.slots[i]);
                } else {
                    slots[i].SetEmpty();
                }
            }
        }
    }

    public void Remove(int slotID)
    {
        // drops item from player's inventory and removes it from the inventory UI
        Collectable itemToDrop = GameManager.instance.itemManager.GetItemByType(player.inventory.slots[slotID].type);
        if (itemToDrop != null)
        {
            player.DropItem(itemToDrop);
            player.inventory.Remove(slotID);
            Refresh();
        }
        
    }

}
