using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // player movement speed
    public float moveSpeed;

    // dictionary for current active tiles
    Dictionary<Vector3Int, int> activeTiles = new Dictionary<Vector3Int, int>();
    private Grid grid;
    private Camera cam;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;
    public GameObject inventoryWindow;
    public GameObject dayNightLighting;
    public GameObject sleepMenu;


    // tiles for farming
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile tilledTile;
    [SerializeField] private Tile wateredTile;
    [SerializeField] private Tile stage1GrowTile;
    [SerializeField] private Tile stage2GrowTile;
    [SerializeField] private Tile stage3GrowTile;
    [SerializeField] private Tile stage4GrowTile;


    // cooldown for using items
    private float useTime = 0.5f;
    private float useCD;
    private bool isUsing = false;   

    // menu
    private bool inMenu = false;

    public InventoryManager inventoryManager;
    public DayNightLighting dayNightTime;

    // variable to change sorting order of roof when player collides with door
    public bool enter = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        grid = GameObject.Find("FarmingSpace").GetComponent<Grid>();

        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible") {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }

        dayNightTime = dayNightLighting.GetComponent<DayNightLighting>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        
    }

    public bool isInteractable(Vector3Int position) {
        TileBase tile = interactableMap.GetTile(position);
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
    
    public void ChangeMenuState() {
        inMenu = !inMenu;
    }
    // controls animations, fetches user input
    private void Update()
    {
        if(!isUsing && !inMenu)
        {
            // movement animations 
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        } else {
            movement = Vector2.zero;
        }
        
        // sets animaton parameters (speed checks when player moving)
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            animator.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            animator.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            inventoryWindow.SetActive(!inventoryWindow.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isUsing && inventoryManager.GetSelectedItem(false) != null && !inMenu) {

            UsingCD();

            Vector3 point = new Vector3();
            Vector3 pointPos = new Vector3();

            pointPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);

            // converts mouse position to world position
            point = cam.ScreenToWorldPoint(pointPos);
            // converts world position to cell position on the grid
            Vector3Int cellPosition = grid.WorldToCell(point);

            TileBase tile = interactableMap.GetTile(cellPosition);
            switch(inventoryManager.GetSelectedItem(false).name) {
                
                // check for null value
                case null:
                    break;

                case "Hoe":
                    if(isInteractable(cellPosition)) {
                        if(activeTiles.Count == 0) {
                            activeTiles.Add(cellPosition, 1);
                            interactableMap.SetTile(cellPosition, tilledTile);
                            // only plays animation if farming space is created
                            animator.Play("PlayerHoe");
                        } else {
                            // if the dictionary has values, check if the current cell position is already in the dictionary
                            if(!activeTiles.ContainsKey(cellPosition))  {
                            
                                activeTiles.Add(cellPosition, 1);
                                interactableMap.SetTile(cellPosition, tilledTile);

                                // only plays animation if farming space is created
                                animator.Play("PlayerHoe");
                            }
                        }
                    }
                    break;

                case "Watering Can":

                    if (tile != null && tile.name == "carrotFarmingTiles_0" && activeTiles.ContainsKey(cellPosition)) { // if the tile is not a blank tile, and the tile name is tilledDirt, and the tile's cell position is in the dictionary
                        activeTiles[cellPosition] = 2;
                        interactableMap.SetTile(cellPosition, wateredTile);
                        animator.Play("Player_WateringCan");
                    } 

                    break;
                
                case "Carrot Seed":

                    if (tile != null && tile.name == "carrotFarmingTiles_1" && activeTiles.ContainsKey(cellPosition)) { // if the tile is not a blank tile, and the tile name is wateredDirt, and the tile's cell position is in the dictionary
                        activeTiles[cellPosition] = 3;
                        interactableMap.SetTile(cellPosition, stage1GrowTile);
                        inventoryManager.GetSelectedItem(true);
                    }

                    break;

            }

        }
        
        //timer for resetting isUsing
        if (isUsing) 
        {
            useCD -= Time.deltaTime;
            if (useCD <= 0) 
            {
                isUsing = false;
            }
        }

    } 

    public void SleepSequence() {
        // grow all crops
        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null) {
                switch(tile.name) {
                    case "carrotFarmingTiles_2":
                        interactableMap.SetTile(position, stage2GrowTile);
                        break;
                    case "carrotFarmingTiles_3":
                        interactableMap.SetTile(position, stage3GrowTile);
                        break;
                    case "carrotFarmingTiles_4":
                        interactableMap.SetTile(position, stage4GrowTile);
                        break;
                }
            }
        }
        // change time to 8am the next day
        dayNightTime.ChangeTime(0, 0, 6, 1, true, false);
    }

    private void UsingCD()
    {
        isUsing = true;
        useCD = useTime;

    }
    
    // moves player using input from Update()
    private void FixedUpdate()
    {
        movement = new Vector2(movement.x, movement.y);
        movement.Normalize();
        rb.velocity = movement * moveSpeed * Time.deltaTime;
    }

    // uses 'enter' variable to determine whether the player is inside the house, and changes the sorting order of the roof accordingly
    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag) {
            case "Door":
                if (enter == true) {
                    enter = false;
                    GameObject.Find("Roof").GetComponent<TilemapRenderer>().sortingOrder = 6;
                } else {
                    enter = true;
                    GameObject.Find("Roof").GetComponent<TilemapRenderer>().sortingOrder = -1;
                }
                break;
            
            // Josh's code for entering and exiting the vendor scene
            case "vendorEntryPoint":
                SceneManager.LoadScene("VendorScene");
                break;

            case "vendorExitPoint":
                SceneManager.LoadScene("FarmScene");
                break;

            // not implemented yet
            case "Bed":
                sleepMenu.SetActive(true);
                inMenu = true;
                break;

        }
         
    }

}
