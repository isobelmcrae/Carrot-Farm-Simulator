// testing spawning collectables
// this is a temp script to test collisions and adding items to inventory
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public DialogueManager dialogueManager;
    // the id of the item
    public int id = 0;
    // if the player collides with the item, add it to the inventory
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PickupManager collectable = GetComponentInParent<PickupManager>();
            collectable.PickupItem(id);
            dialogueManager.tutorialText(id);
            Destroy(this.gameObject);
        } 
    }
}
