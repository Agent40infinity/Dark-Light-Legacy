using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Core:
    public float xSpeed = 12f; //default value for movement on the x-axis.
    public float ySpeed = 12f; //default value for movement on the y-axis.
    private float gravity; //default value of gravity for player.
    private float force; //default value of the force applied to the player.
    private bool isJumping; //Default value of whether the player is jumping.
    private bool isGrounded; //default value for whether the player is on the ground or not.
    public float checkRadius; //Creates a radius to check for the ground.

    //Timers/Counters:
    private float aTTimer; //Air time timer.
    public float airTime = 0.1f; //Air time counter.

    //Reference:
    private Rigidbody2D rigid; //References the RigidBody2D for player.
    public Transform feetPos; //Used to reference the ground check for player.
    public LayerMask isWalkable; //Used to create reference to walkable objects.

	public void Start() 
    {
        rigid = GetComponent<Rigidbody2D>();
	}

    public void Update() 
    {
        Movement();
    }

    public void Movement() //Normal Movement - used for vertical input and movement.
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, isWalkable); //Checks for if the player is grounded or not.
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) //Checks if the player is grounded and space has been pressed - light jump.
        {
            isJumping = true;
            aTTimer = airTime;
            rigid.velocity = Vector2.up * ySpeed;
        }
        if (Input.GetKey(KeyCode.Space) && isJumping == true) //Checks if space has been pressed and that the player is in the air.
        {
            if (aTTimer > 0) //Checks the timer to allow the player to jump higher.
            {
                rigid.velocity = Vector2.up * ySpeed;
                aTTimer -= Time.deltaTime;
            }
            else 
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) //Makes sure jumping isn't active when space isn't pressed.
        {
            isJumping = false;
        }
    }

    public void FixedUpdate()
    {
        MovementF();
    }

	public void MovementF() //Fixed Movement - Allows for horizontal input and movement.
    {
        force = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(force * xSpeed, rigid.velocity.y);
	}
}
