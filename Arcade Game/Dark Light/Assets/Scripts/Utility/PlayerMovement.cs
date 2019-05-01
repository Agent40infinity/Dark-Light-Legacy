using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        #region Variables
        //Core:
        public float xSpeed = 12f; //default value for movement on the x-axis.
        public float ySpeed = 12f; //default value for movement on the y-axis.
        public float yLimiter = 0.5f; //default value for the limiter placed on the y-axis.
        private float gravity; //default value of gravity for player.
        private float force; //default value of the force applied to the player.
        private bool isJumping; //Default value of whether the player is jumping.
        public bool isFacing; //What direction is the player facing? true = right, false = left.
        private bool isGrounded; //default value for whether the player is on the ground or not.
        public float checkRadius; //Creates a radius to check for the ground.
        public float accelSpeed = 4f; //default value for dash's speed.
        public bool dash = false;
		public bool canDash = true;
		public bool dashReset = false;
		public bool dashCooldown = false;
        public bool lockMovement = false;
        public bool lockYAxis = false;
        public bool unlockYAxis = false;
        public Vector2 tempGravity;
        public Vector2 tempYVelocity;

        //Timers/Counters:
        private float aTTimer; //Air time timer.
        public float airTime = 0.1f; //Air time counter.
        public float dashTimer; //Dash time timer
        public float dashTimeReset = 0.15f; //dash time reset
		public float dashCTime = 0.5f;
		public float dCTimer = 0.5f; 

        //Reference:
        public Object playerR;
        private Rigidbody2D rigid; //References the RigidBody2D for player.
        public Transform feetPos; //Used to reference the ground check for player.
        public LayerMask isWalkable; //Used to create reference to walkable objects.
        #endregion

        #region General
        public void Start()
        {
            rigid = GetComponent<Rigidbody2D>();
            dashTimer = dashTimeReset;
            isFacing = true; //Defaults the player to look right.
            tempGravity = Physics2D.gravity;
        }

        public void Update()
        {
            Debug.Log("Gravity before unlock: " + Physics2D.gravity);
            //Debug.Log("Facing Right? " + isFacing);
            //Debug.Log((int)Input.GetAxis("Horizontal"));
            if (lockMovement == false)
            {
                Movement();
            }

            if (dash == true)
            {
                Dash();
            }
        }

        public void FixedUpdate()
        {
            if (lockMovement == false)
            {
                MovementF();
            }
        }
        #endregion

        #region Movement - Update
        public void Movement() //Normal Movement - used for vertical input and movement.
        {
            if ((int)Input.GetAxisRaw("Horizontal") == -1) //Determines whether or not the player is Facing left.
            {
                isFacing = false;

            }
            else if ((int)Input.GetAxisRaw("Horizontal") == 1) //Determines whether or not the player is Facing right.
            {
                isFacing = true;
            }
			if (dashCooldown == true)
			{
				if (dCTimer >= 0)
				{
                    Debug.Log(dCTimer);
					dCTimer -= Time.deltaTime;
				}
				else 
				{
					dCTimer = dashCTime;
					dashReset = true;
					dashCooldown = false;
				}
			}
			if (isGrounded == true && dashReset == true)
			{
				canDash = true;
                dashReset = false;
			}
            if (Input.GetKeyDown(KeyCode.LeftShift)) //Checks whether or not the player is attempting to dash.
            {
				if (canDash == true)
				{
					dash = true;
					lockMovement = true;
				}
            }

            isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, isWalkable); //Checks for if the player is grounded or not.
            if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) //Checks if the player is grounded and space has been pressed - light jump.
            {
                isJumping = true;
                aTTimer = airTime;
                rigid.velocity = Vector2.up * ySpeed * yLimiter;
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
        #endregion

        #region Dash - Update
        public void Dash() //Movement: Dash - Allows the player to dash forward.
        {
            if (dash == true) //Double check that dash is activated.
            {
                if (dashTimer >= 0) //Checks if the dash timer is being counted.
                {
                    //playerR.GetComponent<Player>().IFrame();
                    lockYAxis = true;//enable y axis lock here.
					canDash = false;
                    dashTimer -= Time.deltaTime;
                    if (isFacing == true) //Checks what direction the dash is being activated from and acts accordingly.
                    {
                        force = 1f;
                        rigid.velocity = new Vector2(force * xSpeed * accelSpeed, rigid.velocity.y);
                    }
                    if (isFacing == false)
                    {
                        force = -1f;
                        rigid.velocity = new Vector2(force * xSpeed * accelSpeed, rigid.velocity.y);
                    }
                }
                else //Disables Dash. 
                {
                    unlockYAxis = true; //disable y axis lock here.
                    dashTimer = dashTimeReset;
					lockMovement = false;
					dashCooldown = true;
                    dash = false;
                }
            }

            if (lockYAxis == true)
            {
                //tempGravity = Physics2D.gravity;
                Physics2D.gravity = Vector2.zero;
                //Debug.Log("Gravity: " + Physics2D.gravity);
                tempYVelocity = rigid.velocity;
                rigid.velocity = new Vector2(tempYVelocity.x, 0);
                lockYAxis = false;
            }
            if (unlockYAxis == true)
            {
                //Debug.Log("Gravity before unlock: " + Physics2D.gravity);
                Physics2D.gravity = tempGravity;
                rigid.velocity = tempYVelocity;
                unlockYAxis = false; ;
            }
        }
        #endregion

        #region Movement - Fixed
        public void MovementF() //Fixed Movement - Allows for horizontal input and movement.
        {
            force = Input.GetAxisRaw("Horizontal");
            rigid.velocity = new Vector2(force * xSpeed, rigid.velocity.y);
        }
        #endregion
    }




