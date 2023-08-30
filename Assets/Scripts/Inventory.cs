using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    [System.Serializable]
    public class Slot {
        public CollectableType type;
        public int count;
        public int maxAllowed = 99;
        public Sprite icon;

        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            
        }

        // checks if the slot can add an item
        public bool CanAddItem()
        {
            if(count < maxAllowed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // adds item to slot
        public void AddItem(Collectable item)
        {
            this.type = item.type;
            this.icon = item.icon;
            count++;
        }

        // removes item from slot
        public void RemoveItem()
        {
            if(count > 0)
            {
                count--;

                if(count == 0)
                {
                    type = CollectableType.NONE;
                    icon = null;
                }
            } 
        }

    }

    // list of slots in inventory
    public List<Slot> slots = new List<Slot>();

    // creates inventory with numSlots slots
    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++) 
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
        
    }

    // adds item to inventory
    public void Add(Collectable item) 
    {
        
        foreach(Slot slot in slots)
        {
            // checks if slot type is the same as item type and if the slot can add an item
            if(slot.type == item.type && slot.CanAddItem())
            {
                slot.AddItem(item);
                return;
            }
        }

        // if no slot can add an item, add item to empty slot
        foreach(Slot slot in slots)
        {
            // if the slot type is none, add item to slot
            if(slot.type == CollectableType.NONE)
            {
                slot.AddItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoveItem();
    }
}
   