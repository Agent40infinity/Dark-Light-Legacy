using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerMovementSwitch : MonoBehaviour
    {
    #region Variables
    //Core:
    //public YoloMotherFucker locks;
        public float xSpeed = 12f; //Default value for movement on the x-axis.
        public float ySpeed = 14f; //Default value for movement on the y-axis.
        public float yLimiter = 0.5f; //Default value for the limiter placed on the y-axis.
        private float gravity; //Default value of gravity for player.
        private float force; //Default value of the force applied to the player.
        private bool isJumping; //Default value of whether the player is jumping.
        public bool isFacing; //What direction is the player facing? true = right, false = left.
        public bool isGrounded; //Default value for whether the player is on the ground or not.
        public float checkRadius; //Creates a radius to check for the ground.
        public float accelSpeed = 4f; //Default value for dash's speed.
        public bool dash = false; //Used to call upon the sub-routine (dash).
		public bool canDash = true; //Used to check whether or not the player is able to dash.
		public bool dashReset = false; //Used to see whether or not the dash can be reset.
		public bool dashCooldown = false; //Used to see whether or not the dash is on cooldown.
        public bool lockMovement = false; //Used to check whether or not all movment is required to be locked.
        public bool lockYAxis = false; //Used to lock the Y Axis.
        public bool unlockYAxis = false; //Used to Unlock the Y Axis.
        public Vector2 tempGravity; //Used to temporarily store the value of gravity.
        public Vector2 tempYVelocity; //Used to temporarily store the value of the Y velocity.

        //Timers/Counters:
        private float aTTimer; //Air time timer.
        public float airTime = 0.1f; //Air time counter.
        public int aHTimer;
        public int airHTime = 5;
        public float dashTimer; //Dash time timer.
        public float dashTimeReset = 0.15f; //Dash time reset.
		public float dashCTime = 0.5f; //Dash cooldown reset.
		public float dCTimer = 0.5f; //Dash cooldown timer

        //Reference:
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
			if (dashCooldown == true)  //Puts dash on cooldown.
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
			if (isGrounded == true && dashReset == true) //Resets the Dash 
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
                rigid.velocity = Vector2.up * ySpeed * yLimiter;
                aTTimer = airTime;
                isJumping = true;
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
                Vector2 jX = rigid.velocity;
                if (rigid.velocity.y >= yLimiter)
                {
                    rigid.velocity = new Vector2(jX.x, 0);
                    isJumping = false;
                }

            }
        }
        #endregion

        #region Dash - Update
        public void Dash() //Movement: Dash - Allows the player to dash forward.
        {
        if (dash == true) //Double check that dash is activated.S
        {
            if (dashTimer >= 0) //Checks if the dash timer is being counted.
            {
                GetComponent<Player>().IFrame();
                lockYAxis = true;//enable y axis lock here.
                                 //locks = YoloMotherFucker.yLocked;
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
                //locks = YoloMotherFucker.xLocked;

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
            unlockYAxis = false;
        }
        //switch (locks)
        //    {
        //    case YoloMotherFucker.unlocked:

        //    break;
        //    case YoloMotherFucker.yLocked:
        //        tempGravity = Physics2D.gravity;
        //        Physics2D.gravity = Vector2.zero;
        //        //Debug.Log("Gravity: " + Physics2D.gravity);
        //        tempYVelocity = rigid.velocity;
        //        rigid.velocity = new Vector2(tempYVelocity.x, 0);
        //        lockYAxis = false;
        //        break;
        //    case YoloMotherFucker.xLocked:
        //        Debug.Log("Gravity before unlock: " + Physics2D.gravity);
        //        Physics2D.gravity = tempGravity;
        //        rigid.velocity = tempYVelocity;
        //        unlockYAxis = false;
        //        break;
        //    case YoloMotherFucker.locked:

        //        break;

        //}

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
//public enum YoloMotherFucker
//{
//    unlocked,
//    yLocked,
//    xLocked,
//    locked
//}




