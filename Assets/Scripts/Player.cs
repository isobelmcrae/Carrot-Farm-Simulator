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
    Dictionary<Vector3Int, string> activeTiles = new Dictionary<Vector3Int, string>();
    private Grid grid;
    private Camera cam;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;

    // tiles for farming
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile tilledTile;
    [SerializeField] private Tile wateredTile;

    // cooldown for using items
    private float useTime = 0.1f;
    private float useCD;
    private bool isUsing = false;   

    public InventoryManager inventoryManager;

    // variable to change sorting order of roof when player collides with door
    public bool enter = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        grid = GameObject.Find("FarmingGrid").GetComponent<Grid>();

        foreach(var position in interactableMap.cellBounds.allPositionsWithin) {
            TileBase tile = interactableMap.GetTile(position);
            if (tile != null && tile.name == "Interactable_Visible") {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }

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

    // controls animations, fetches user input
    private void Update()
    {
        if(!isUsing)
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isUsing) 
        {
            UsingCD(); // triggers cooldown before player can input again
            // farming 
            if(inventoryManager.GetSelectedItem(false).name == "Hoe") // if the item is a hoe, don't "use" the item but till the tile
            {
                Vector3 point = new Vector3();
                Vector3 pointPos = new Vector3();

                pointPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);

                // converts mouse position to world position
                point = cam.ScreenToWorldPoint(pointPos);
                // converts world position to cell position on the grid
                Vector3Int cellPosition = grid.WorldToCell(point);
                
                if(isInteractable(cellPosition)) {
                    if(activeTiles.Count == 0) {
                            activeTiles.Add(cellPosition, "tilled");
                            interactableMap.SetTile(cellPosition, tilledTile);
                            // only plays animation if farming space is created
                            animator.Play("PlayerHoe");
                    } else {
                        // if the dictionary has values, check if the current cell position is already in the dictionary
                        if(!activeTiles.ContainsKey(cellPosition))  {

                            activeTiles.Add(cellPosition, "tilled");
                            interactableMap.SetTile(cellPosition, tilledTile);

                            // only plays animation if farming space is created
                            animator.Play("PlayerHoe");
                        }
                    }
                }

            } else if(inventoryManager.GetSelectedItem(false).name == "WateringCan") // if the item is a watering can, don't "use" the item but water the tile
            {
                Vector3 point = new Vector3();
                Vector3 pointPos = new Vector3();
                pointPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);

                // converts mouse position to world position
                point = cam.ScreenToWorldPoint(pointPos);
                // converts world position to cell position on the grid
                Vector3Int cellPosition = grid.WorldToCell(point);
                TileBase tile = interactableMap.GetTile(cellPosition);

                if (tile != null && tile.name == "tilledDirt" && activeTiles.ContainsKey(cellPosition)) { // if the tile is not a blank tile, and the tile name is tilledDirt, and the tile's cell position is in the dictionary
                    activeTiles[cellPosition] = "watered";
                    interactableMap.SetTile(cellPosition, wateredTile);
                }
                
            } else if(inventoryManager.GetSelectedItem(true).name == "CarrotSeed") {

                Vector3 point = new Vector3();
                Vector3 pointPos = new Vector3();
                pointPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane);

                // converts mouse position to world position
                point = cam.ScreenToWorldPoint(pointPos);
                // converts world position to cell position on the grid
                Vector3Int cellPosition = grid.WorldToCell(point);
                TileBase tile = interactableMap.GetTile(cellPosition);

                if (tile != null && tile.name == "wateredDirt" && activeTiles.ContainsKey(cellPosition)) { // if the tile is not a blank tile, and the tile name is tilledDirt, and the tile's cell position is in the dictionary
                    activeTiles[cellPosition] = "carrot1_stage1";
                    interactableMap.SetTile(cellPosition, wateredTile);
                }
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
        if (other.gameObject.CompareTag("Door"))
        {
            if (enter == true) {
                enter = false;
                GameObject.Find("House_Roof").GetComponent<TilemapRenderer>().sortingOrder = 6;
            } else {
                enter = true;
                GameObject.Find("House_Roof").GetComponent<TilemapRenderer>().sortingOrder = -1;
            }
        // Josh's code for entering and exiting the vendor scene
        } else if (other.gameObject.CompareTag("vendorEntryPoint")) {
            SceneManager.LoadScene("VendorScene");
        } else if (other.gameObject.CompareTag("vendorExitPoint")) {
            SceneManager.LoadScene("FarmScene");
        }
    }

}

