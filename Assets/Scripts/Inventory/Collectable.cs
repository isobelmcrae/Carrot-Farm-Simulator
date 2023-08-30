using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Collectable : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if the other object is the player, add this item to the player's inventory and destroy this object
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();

            Item item = GetComponent<Item>();

            if(item != null)
            {
                player.inventory.Add(item);
                Destroy(this.gameObject);
            }
            
        }
    }

}
 