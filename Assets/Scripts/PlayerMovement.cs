using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // need to create a reference to rigid body to use rigid body component in unity
    // rigid body is used to apply forces (move player left and right)
    private Rigidbody2D rbody;

    // we can change speed in unity (for now set at 10)
    [SerializeField] private float speed;

    // need to refernce to use box collider component
    private BoxCollider2D boxCollider;

    // to add ground as a layer
    [SerializeField] private LayerMask groundLayer;

    // Will be updated with player input
    private float horizontalInput = 0f;

    // Creating reference to animator - Matias
    private Animator anim;


    private void Awake()
    {
        // will check the player object if it has a rigid body component
        // and then will store the component in rbody variable
        rbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        // Stores the animator in the anim variable - Matias
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal"); // x coordinate value

        // use velocity to change how fast player moves and in what direction
        // Vector2 to change movement in 2 directions
        // x coord is left and right
        // y coord is up and down
        // change the x coordinate, keep the y coordinate the same
        rbody.velocity = new Vector2(horizontalInput * speed, rbody.velocity.y);

        // flip player if they move left or right
        if (horizontalInput > 0.01f) // moving right
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (horizontalInput < -0.01f) // moving left
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

             
        // check if space bar is used to jump and if on the ground
        if (Input.GetKey(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        // Check for when to switch from idle to running and vice versa - Matias
        UpdateAnimation();

    }

    private void Jump()
    {
        // change the y coordinate, keep the x coordinate the same
        rbody.velocity = new Vector2(rbody.velocity.x, 7.0f);
    }

    // to make sure you can only jump when on the ground (to prevent jumping out of screen)
    private bool IsGrounded()
    {
        // will determine if an object collider has been hit (in this case the ground)
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        // checks if anything is under the object (false if nothing, true if the ground is under/raycastHit)
        return raycastHit.collider != null;
    }

    // Checks to see if character is running or not so Animator can select the correct animation - Matias
    private void UpdateAnimation()
    {
        if (horizontalInput > 0.01f) // moving right
        {
            anim.SetBool("Running", true);
        }
        else if (horizontalInput < -0.01f) // moving left
        {
            anim.SetBool("Running", true);
        }
        else // Not Moving
        {
            anim.SetBool("Running", false);
        }
    }
}

