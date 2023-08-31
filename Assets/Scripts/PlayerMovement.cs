using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // player movement speed
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;

    private float useTime = 0.4f;
    private float useCD;
    private bool isUsing = false;

    public InventoryManager inventoryManager;

    // variable to change sorting order of roof when player collides with door
    public bool enter = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // controls animations, fetches user input
    private void Update()
    {
        if(!isUsing)
        {
            // movement animations 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        }
        else
        {
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

        if(Input.GetKeyDown(KeyCode.Mouse0) && !isUsing) 
        {
            UsingCD(); //triggers cooldown before player can input again
            if(inventoryManager.GetSelectedItem(false).name == "Hoe") 
            {
                animator.Play("PlayerHoe");
                
            } 
            else
            {
                return;
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
        }
    }

}

