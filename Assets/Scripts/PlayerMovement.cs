using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // character movespeed
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;

    // variable to change sorting order of roof when player collides with door
    public bool enter = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // animations + getting user input
    private void Update()
    {
        // movement animations 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // sets animaton parameters (speed checks when player moving)
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }
    
    // moves character
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

