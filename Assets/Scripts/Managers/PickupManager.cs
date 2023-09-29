using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;

    // adds item to inventoryManager with the given id, returns true if successful 
    public void PickupItem(int id) {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
    }
}
