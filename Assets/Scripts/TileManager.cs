using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap Plots_InteractableMap;
    
    [SerializeField] private Tile hiddenInteractableTile;

    void Start()
    {
        foreach(var position in Plots_InteractableMap.cellBounds.allPositionsWithin)
        {
            Plots_InteractableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    /* public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = Plots_InteractableMap.GetTile(position);
        if(tile != null)
        {
            if(tile.name == "Interactable")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    } */

}
