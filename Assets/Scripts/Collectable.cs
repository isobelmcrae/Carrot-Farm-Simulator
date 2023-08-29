using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public CollectableType type;

    private void OnTriggerEnter2D(Collider2D other) 
    {
         if (other.gameObject.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();

            player.numCarrotSeed++;
            Destroy(this.gameObject);
         }
    }

    public enum CollectableType 
    {
        NONE, CARROT_SEED
    }


}
 