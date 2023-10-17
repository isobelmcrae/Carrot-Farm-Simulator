using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Farming")] // farming variables
    
    private Grid grid;
    private Camera cam;

    Dictionary<Vector3Int, int> activeTiles = new Dictionary<Vector3Int, int>();
    [SerializeField] private Tilemap interactableMap;
    
    [Header("Tiles")]

    [SerializeField] private Tile hiddenInteractable;
    [SerializeField] private Tile tilled;
    [SerializeField] private Tile watered;
    [SerializeField] private Tile stage1Grow;
    [SerializeField] private Tile stage2Grow;
    [SerializeField] private Tile stage3Grow;
    [SerializeField] private Tile stage4Grow;

    [Header("Time")]

    public GameObject dayNightLighting;
    public InventoryManager inventoryManager;
    public DayNightLighting dayNightTime;

    // indicates when a player is in a menu
    public bool inMenu = false;

    // variable to change sorting order of roof when player collides with door
    public bool inHouse = false;

    private void Start() {
        cam = Camera.main;
        grid = GameObject.Find("FarmingSpace").GetComponent<Grid>();

        // interactable white tiles indicate which spaces can be interacted with, but are hidden from the player on startup
        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible") {
                interactableMap.SetTile(position, hiddenInteractable);
            }
        }

        dayNightTime = dayNightLighting.GetComponent<DayNightLighting>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // sets inMenu variable to true or false based on function input
    public void MenuState(bool state) {
        inMenu = state;
    }

    // checks if a tile can be interacted with
    public bool isInteractable(Vector3Int position) {
        
        TileBase tile = interactableMap.GetTile(position);
        // checks if the tile is null, as interactable spaces are just set to transparent pngs
        if (tile != null) {
            if (tile.name == "Interactable") {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }

    }

    public void SleepSequence() {

        growCrops();
        // change time to 8am the next day
        dayNightTime.ChangeTime(0, 0, 6, 1, true, false);

    }

    public Vector3Int mouseToTilePosition() {
        
        Vector3 point = new Vector3();
        Vector3 pointPos = new Vector3();

        // gets mouse position
        pointPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);

        // converts mouse position to world position
        point = cam.ScreenToWorldPoint(pointPos);
        Vector3Int cellPosition = grid.WorldToCell(point);

        return cellPosition;
    }

    public void addTile(Vector3Int cellPosition, string tileName) {
        TileBase tile = interactableMap.GetTile(cellPosition);

        switch(tileName) {
            
            case "tilled":
                if(activeTiles.Count == 0) {
                    activeTiles.Add(cellPosition, 1);
                    interactableMap.SetTile(cellPosition, tilled);

                } else if (!activeTiles.ContainsKey(cellPosition)){
                    activeTiles.Add(cellPosition, 1);
                    interactableMap.SetTile(cellPosition, tilled);
                } 

                break;

            case "watered":

                if (tile != null && tile.name == "carrotFarmingTiles_0" && activeTiles.ContainsKey(cellPosition)) {
                    activeTiles[cellPosition] = 2;
                    interactableMap.SetTile(cellPosition, watered);
                }

                break;
            
            case "stage1Grow":
    
                if (tile != null && tile.name == "carrotFarmingTiles_1" && activeTiles.ContainsKey(cellPosition)) {
                    activeTiles[cellPosition] = 3;
                    interactableMap.SetTile(cellPosition, stage1Grow);
                }
    
                break;
        }    
    }

    public void growCrops() {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null) {
                switch(tile.name) {
                    case "carrotFarmingTiles_2":
                        interactableMap.SetTile(position, stage2Grow);
                        break;
                    case "carrotFarmingTiles_3":
                        interactableMap.SetTile(position, stage3Grow);
                        break;
                    case "carrotFarmingTiles_4":
                        interactableMap.SetTile(position, stage4Grow);
                        break;   
                }
            }
        }
    }
}
