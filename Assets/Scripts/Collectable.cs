using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    public Sprite icon;

    public Rigidbody2D rb2d;
    
    private void Awake() 
    {
        rb2d = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if the other object is the player, add this item to the player's inventory and destroy this object
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();

            player.inventory.Add(this);
            Destroy(this.gameObject);
        }
    }

}

// enum for collectable types
public enum CollectableType 
{
    NONE, CARROT_SEED
}
 