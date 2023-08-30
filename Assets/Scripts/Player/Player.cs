using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public TileManager tileManager;
    [SerializeField] private GameObject tilledTile;

    // creates inventory with 16 slots
    private void Awake()
    {
        inventory = new Inventory(16);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int playerPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if(GameManager.instance.tileManager.IsInteractable(playerPos))
            {
                Debug.Log("Interactable");
                Instantiate(tilledTile, playerPos, Quaternion.identity);
            }
        }
    }

    // drops inventory item at player position with random offset
    public void DropItem(Item item) {
        Vector3 spawnLocation = transform.position;

        float randX = Random.Range(-1f, 1f);
        float randY = Random.Range(-1f, 1f);

        // creates a spawn offset vector with random x and y values
        Vector3 spawnOffset = new Vector3(randX, randY, 0f).normalized;

        // instantiates item at player position with spawn offset
        Item droppedItem = Instantiate(item, spawnLocation + spawnOffset, Quaternion.identity); 
    }
}
