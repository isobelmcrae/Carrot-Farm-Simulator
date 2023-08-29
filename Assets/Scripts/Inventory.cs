using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public class Slot {
        public CollectableType type;
        public int count;
        public int maxAllowed;

        public Slot()
        {
            type = CollectableType.NONE;
            count = 0;
            
        }
    }

    public List<Slot> slots = new List<Slot>();

    public Inventory(int numSlots)
    {
        for(int i = 0; i < numSlots; i++) 
        {
            Slot slot = new Slot();
            slots.Add(slot);
        }
        
    }
}
   