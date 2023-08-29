using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    public TileManager tileManager;

    private void Awake()
    {
        inventory = new Inventory(16);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3Int mousePos = new Vector3Int((int)Input.mousePosition.x, (int)Input.mousePosition.y, 0);
            Vector3Int playerPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            // if(TileManager.instance.tileManager.IsInteractable(playerPos))
            {
                Debug.Log("Interactable");
            }
        }
    }

    // tileManager = GetComponent<TileManager>();

}
