using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Collectable[] collectableItems;

    // creates dictionary to store collectable items by type
    private Dictionary<CollectableType, Collectable> collectableItemsDict = new Dictionary<CollectableType, Collectable>();

    private void Awake()
    {
        // adds each collectable item from collectableItems to the dictionary
        foreach(Collectable item in collectableItems)
        {
            AddItem(item);
        }
    }

    // adds item to dictionary if it does not already exist in the dictionary
    private void AddItem(Collectable item)
    {
        if(!collectableItemsDict.ContainsKey(item.type))
        {
            collectableItemsDict.Add(item.type, item);
        }
        
    }

    // returns item from dictionary by type
    public Collectable GetItemByType(CollectableType type)
    {
        // checks if dictionary contains item type
        if(collectableItemsDict.ContainsKey(type))
        {
            return collectableItemsDict[type];
        }
        
        return null;
        
    }
}
