using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameManager game;
    private Camera cam;

    // player movement speed
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;
    public GameObject inventoryWindow;
    public GameObject sleepMenu;

    // cooldown for using items
    private float useTime = 0.5f;
    private float useCD;
    private bool isUsing = false;

    public InventoryManager inventoryManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        inventoryManager = FindObjectOfType<InventoryManager>();
        
    }
    
    // controls animations, fetches user input
    private void Update()
    {
        if(!isUsing && !game.inMenu)
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isUsing && inventoryManager.GetSelectedItem(false) != null && !game.inMenu) {

            Vector3Int cellPosition = game.mouseToTilePosition();
            UsingCD();
            switch(inventoryManager.GetSelectedItem(false).name) {
                
                // check for null value
                case null:
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    }
                    break;

                case "Hoe":

                    if(game.isInteractable(cellPosition)) {
                        game.addTile(cellPosition, "tilled");
                        animator.Play("PlayerHoe");
                    } else {
                        if (game.isHarvestable(cellPosition)) {
                            game.harvest(cellPosition);
                        }
                    }

                    break;

                case "Watering Can":

                    game.addTile(cellPosition, "watered");
                    animator.Play("Player_WateringCan");

                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    }
                    break;
                
                case "Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1CarrotGrow");
                    }

                    break;

                case "Baby Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1BabyGrow");
                    }

                    break;
                
                case "Dirty Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1DirtyGrow");
                    }

                    break;

                case "Muscle Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1MuscleGrow");
                    }

                    break;
                
                case "Princess Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1PrincessGrow");
                    }

                    break;

                case "Lovers Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1LoversGrow");
                    }

                    break;
                
                case "Super Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1SuperGrow");
                    }

                    break;

                case "Golden Carrot Seed":
                    if (game.isHarvestable(cellPosition)) {
                        game.harvest(cellPosition);
                    } else {
                        // checks if circumstances are perfect for seed planting
                        game.addTile(cellPosition, "stage1GoldenGrow");
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
                if (game.inHouse == true) {
                    game.inHouse = false;
                    GameObject.Find("Roof").GetComponent<TilemapRenderer>().sortingOrder = 6;
                } else {
                    game.inHouse = true;
                    GameObject.Find("Roof").GetComponent<TilemapRenderer>().sortingOrder = -1;
                }
                break;

            // not implemented yet
            case "Bed":
                sleepMenu.SetActive(true);
                game.inMenu = true;
                break;

            case "vendorEntryPoint":
                game.VendorSetup();
                break;
            
            case "vendorExitPoint":
                game.FarmSetup();
                break;

        }
         
    }

}
