using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Linq;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    [Header("Farming")] // farming variables
    
    private Grid grid;
    private Camera cam;

    public Dictionary<Vector3Int, string> activeTiles = new Dictionary<Vector3Int, string>();
    public Tilemap interactableMap;
    
    [Header("Tiles")]

    [SerializeField] private Tile hiddenInteractable;

    public Tile[] tiles;

    [Header("Time")]

    public GameObject dayNightLighting;
    public DayNightLighting dayNightTime;

    [Header("Vendor")]
    public Volume ppv;
    public Camera vendorCam;    
    public Light2D globalLight;

    // vendor scene player spotlights
    public Light2D playerSpotlight1;
    public Light2D playerSpotlight2;

    [Header("Quests")]

    [Header("UI")]
    public UIManager ui;
    public TMP_Text coins;

    [Header("Items")]
    public Item carrot;
    public Item lovers;
    public Item baby;
    public Item dirty;
    public Item muscle;
    public Item princess;
    public Item hero;

    public Item hoe;
    public Item wateringCan;
    public Item carrotSeed;
    
    [Header("Misc")]

    public int money = 0;
    public InventoryManager inventoryManager;
    public GameObject endDayMenu;

    // indicates when a player is in a menu
    public bool inMenu = false;

    // variable to change sorting order of roof when player collides with door
    public bool inHouse = false;

    private void Start() {
        // sets cam to main camera, finds the grid, and disables the vendor camera
        cam = Camera.main;
        grid = GameObject.Find("FarmingSpace").GetComponent<Grid>();
        vendorCam.enabled = false;

        // disables spotlights for vendor scene
        playerSpotlight1.enabled = false;
        playerSpotlight2.enabled = false;

        // interactable white tiles indicate which spaces can be interacted with, but are hidden from the player on startup
        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible") {
                interactableMap.SetTile(position, hiddenInteractable);
            }
        }

        dayNightTime = dayNightLighting.GetComponent<DayNightLighting>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        inventoryManager.AddItem(hoe);
        inventoryManager.AddItem(wateringCan);

        for (int i = 0; i < 10; i++) {
            inventoryManager.AddItem(carrotSeed);
        }
    }

    private void Update() {
        coins.text = money.ToString();
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
    
    // checks if the carrot's stage is the highest stage (therefore is harvestable)
    public bool isHarvestable(Vector3Int position) {
        TileBase tile = interactableMap.GetTile(position);
        var endStages = new [] {"5", "9", "13", "17", "21", "25", "29", "33"};

        if (tile != null && endStages.Any(tile.name.EndsWith)) {
            return true;
        } else {
            return false;
        }
    }

    
   // harvests tile at position if harvestable
    public void harvest(Vector3Int position, string type) {
        TileBase tile = interactableMap.GetTile(position);
        if (isHarvestable(position)) {
            switch (type) {
                case "carrot":
                    inventoryManager.AddItem(carrot);
                    break;

                case "lovers":
                    inventoryManager.AddItem(lovers);
                    break;

                case "baby":
                    inventoryManager.AddItem(baby);
                    break;

                case "dirty":
                    inventoryManager.AddItem(dirty);
                    break;
                
                case "muscle":
                    inventoryManager.AddItem(muscle);
                    break;

                case "princess":
                    inventoryManager.AddItem(princess);
                    break;
                
            }
            removeTile(position);
        } 
    }
    
    // sets up the vendor area using the fade to/from black effect
    IEnumerator vendorSet() {
        ui.fadeToBlack(true);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 0.16f;
        ppv.enabled = false;
        cam.enabled = false;
        vendorCam.enabled = true;
        player.transform.position = new Vector3(-30, -3.5f, 0);

        playerSpotlight1.enabled = true;
        playerSpotlight2.enabled = true;
        ui.fadeToBlack(false);
    }

    // sets up the farm using the fade to/from black effect
    IEnumerator farmSet() {
        ui.fadeToBlack(true);
        yield return new WaitForSeconds(0.5f);
        globalLight.intensity = 1;
        ppv.enabled = true;
        cam.enabled = true;
        vendorCam.enabled = false;
        player.transform.position = new Vector3(-7.5f, 19.5f, 0);

        playerSpotlight1.enabled = false;
        playerSpotlight2.enabled = false;
        ui.fadeToBlack(false);
    }

    // fade to black effect for sleeping
    IEnumerator sleepFade() {
        ui.fadeToBlack(true);
        yield return new WaitForSeconds(0.5f);
        growCrops();
        // change time to 8am the next day
        dayNightTime.ChangeTime(0, 0, 6, 1, true, false);
        ui.fadeToBlack(false);
    }
    
    // triggers the fade to black effect for sleeping
    public void SleepSequence() {
        StartCoroutine(sleepFade());
    }

    // adds money to player
    public void AddMoney(int money) {
        this.money += money;
    }

    // removes money from player
    public bool RemoveMoney(int money) {
        if (this.money - money < 0) {
            return false;
        } else {
            this.money -= money;
            return true;
        }
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

    // removes a tile (primarily for harvesting)
    public void removeTile(Vector3Int cellPosition) {
        activeTiles.Remove(cellPosition);
        interactableMap.SetTile(cellPosition, hiddenInteractable);
    }

    // adds a tile to the interactable map
    public void addTile(Vector3Int cellPosition, string tileName, string type) {
        TileBase tile = interactableMap.GetTile(cellPosition);
        
        switch(tileName) {
            case "tilled":
                if(activeTiles.Count == 0) {
                    activeTiles.Add(cellPosition, null);
                    interactableMap.SetTile(cellPosition, tiles[0]);

                } else if (!activeTiles.ContainsKey(cellPosition)){
                    activeTiles.Add(cellPosition, null);
                    interactableMap.SetTile(cellPosition, tiles[0]);
                } 

                break;

            case "watered":

                if (tile != null && tile == tiles[0] && activeTiles.ContainsKey(cellPosition)) {
                    interactableMap.SetTile(cellPosition, tiles[1]);
                }

                break;
            
            case "stage1Grow":

                if (tile != null && tile == tiles[1] && activeTiles.ContainsKey(cellPosition)) {
                    activeTiles[cellPosition] = type;

                    switch(type) {
                        case "carrot":
                            interactableMap.SetTile(cellPosition, tiles[2]);
                            break;
                        case "baby":
                            interactableMap.SetTile(cellPosition, tiles[6]);
                            break;
                        case "dirty":
                            interactableMap.SetTile(cellPosition, tiles[10]);
                            break;
                        case "muscle":
                            interactableMap.SetTile(cellPosition, tiles[14]);
                            break;
                        case "princess":
                            interactableMap.SetTile(cellPosition, tiles[18]);
                            break;
                        case "lovers":
                            interactableMap.SetTile(cellPosition, tiles[22]);
                            break;
                        case "super":
                            interactableMap.SetTile(cellPosition, tiles[26]);
                            break;
                        case "golden":
                            interactableMap.SetTile(cellPosition, tiles[30]);
                            break;
                    }
                    // removes carrot seed from inventory after planting
                    inventoryManager.GetSelectedItem(true);
                }
    
                break;
            }
        }


    // fade to black effect for the vendor scene
    public void VendorSetup() {
        StartCoroutine(vendorSet());
    } 

    public void FarmSetup() {
        StartCoroutine(farmSet());
    }

    public void growCrops() {

        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {

            TileBase tile = interactableMap.GetTile(position);
            // the indexes of all of the final stages of the crops
            var endStages = new [] {"0", "1", "5", "9", "13", "17", "21", "25", "29", "33"};

            // checks if the tile is a crop and not in the final stage
            if (tile != null && tile.name != "Interactable" && !endStages.Any(tile.name.EndsWith)) {
                // pulls numbers from tile name
                int index = int.Parse(Regex.Match(tile.name, @"\d+$", RegexOptions.RightToLeft).Value);
                // sets the tile to the next stage from the tiles array
                interactableMap.SetTile(position, tiles[index + 1]);

            }
        }
    }
}
