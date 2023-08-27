// kayla i removed the GetInput() function 
// because i thought it could be incorporated into the FixedUpdate thing,
// but if you think there's an issue with that that i've missed
// lmk and ill change it back ty :)
// - iso

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // character movespeed
    public float moveSpeed;

    private Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;

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

    

}

