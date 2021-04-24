using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script created by Aiden Nathan.
 *---------------------------------*/

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    //Core:
    public float xSpeed = 12f; //Default value for movement on the x-axis.
    public float ySpeed = 14f; //Default value for movement on the y-axis.
    public float yLimiter = 0.5f; //Default value for the limiter placed on the y-axis.
    private float force; //Default value of the force applied to the player.
    public Vector2 knockback = new Vector2(10, 5); //Stores the values for the amount of knockback the player will take.
    public bool beenKnocked = false; //Checks whether or not the player needs to take knockback.
    public bool isJumping; //Default value of whether the player is jumping.
    public bool isFacing; //What direction is the player facing? true = right, false = left.
    public bool isGrounded; //Default value for whether the player is on the ground or not.
    public bool knockbackDirection; //true = left, false = right.
    public Vector2 checkRadius = new Vector2(0.9f, 0.1f); //Stores the values for the range isGrounded will be calculated with.
    public float accelSpeed = 4f; //Default value for dash's speed.
    public bool dash = false; //Used to call upon the sub-routine (dash).
    public bool canDash = true; //Used to check whether or not the player is able to dash.
    public bool dashReset = false; //Used to see whether or not the dash can be reset.
    public int dashAvaliable = 1;
    public bool dashCooldown = false; //Used to see whether or not the dash is on cooldown.
    public bool lockMovement = false; //Used to lock all Movement inputs.
    public LockState lockState = LockState.decease; //Used to lock and unlock all movement, actions, and abilities.
    public bool lockAbilities = false; //Used to lock Abilities.
    public bool lockYAxis = false; //Used to lock the Y Axis.
    public bool unlockYAxis = false; //Used to Unlock the Y Axis.
    public Vector2 tempGravity; //Used to temporarily store the value of gravity.
    public Vector2 tempYVelocity; //Used to temporarily store the value of the Y velocity.

    //Timers/Counters:
    private float aTTimer; //Air time timer.
    public float airTime = 0.1f; //Air time counter.
    //public int aHTimer; //This was for something, can't remember what.
    //public int airHTime = 5; //same with this.
    private float dashTimer; //Dash time timer.
    public float dashTimeReset = 0.15f; //Dash time reset.
    public float dashCTime = 0.5f; //Dash cooldown reset.
    public float dCTimer = 0.5f; //Dash cooldown timer.
    public float knockbackTime = 0.2f; //Knockback time reset.
    public float kBTimer; //Knockback timer.
    public float fallTime; //Fall time counter.
    public int landTime; //Land time counter.

    //Reference:
    public Rigidbody2D rigid; //References the RigidBody2D for player.
    public Transform feetPos; //Used to reference the ground check for player.
    public LayerMask isWalkable; //Used to create reference to walkable objects.
    public GameObject player; //References the player itself.
    public static Transform enemyPos;

    //Particles:
    public GameObject jumpDust;
    #endregion

    #region General
    public void Start() //Defaults values not already set via soft-coding or within the constructor.
    {
        rigid = GetComponent<Rigidbody2D>();
        dashTimer = dashTimeReset;
        kBTimer = knockbackTime;
        isFacing = true; //Defaults the player to look right.
        tempGravity = Physics2D.gravity;
    }

    public void Update()
    {
        #region Debug Logs
        //Debug.Log("iFrame from PlayerMovement: " + player.GetComponent<Player>().iFrame);
        //Debug.Log("fallTime:" + fallTime);
        //Debug.Log("Gravity before unlock: " + Physics2D.gravity);
        //Debug.Log("Facing Right? " + isFacing);
        //Debug.Log((int)Input.GetAxis("Horizontal"));
        //Debug.Log("forceY: " + GetComponent<Player>().anim.GetFloat("forceY") + "   velocity: " + rigid.velocity.y);
        //Debug.Log("Dash: " + dashAvaliable);
        Debug.Log("Velocity:" + rigid.velocity.x + " | Force * Speed: " + (force * xSpeed));
        #endregion

        //Animation controls:
        GetComponent<Player>().anim.SetBool("isGrounded", isGrounded);
        GetComponent<Player>().anim.SetFloat("forceY", rigid.velocity.y);
        GetComponent<Player>().anim.SetBool("Dash", dash);
        GetComponent<Player>().anim.SetBool("isWalking", rigid.velocity.x != 0 && isGrounded);

        if (lockMovement == false)
        {
            Movement();
        }

        if (dash == true && lockAbilities == false /*&& GetComponent<Player>().dashUnlocked == true*/)
        {
            Dash();
        }

        if (beenKnocked == true)
        {
            Knockback();
        }
    }

    public void FixedUpdate()
    {
        Locks();
        if (lockMovement == false)
        {
            MovementF();
        }
    }
    #endregion

    #region Movement - Update
    public void Movement() //Normal Movement - used for vertical input and movement.
    {
        #region Facing
        if (Input.GetKey(GameManager.keybind["Left"])) //Determines whether or not the player is Facing left.
        {
            isFacing = false;
            GetComponent<Player>().rend.flipX = true; //Esed to flip all sprites used in the sprite renderer.
        }
        if (Input.GetKey(GameManager.keybind["Right"])) //Determines whether or not the player is Facing right.
        {
            isFacing = true;
            GetComponent<Player>().rend.flipX = false;
        }
        #endregion

        #region Dash - Movement
        if (dashCooldown == true)  //Puts dash on cooldown.
        {
            if (dCTimer >= 0) //Counts down the cooldown.
            {
                //Debug.Log(dCTimer);
                dCTimer -= Time.deltaTime;
            }
            else //Takes dash off cooldown.
            {
                dCTimer = dashCTime;
                dashReset = true;
                dashCooldown = false;
            }
        }
        if (isGrounded == true && dashAvaliable <= 0)
        {
            dashAvaliable = 1;
        }
        if (dashAvaliable == 1 && dashReset == true) //Resets the Dash 
        {
            canDash = true;
            dashReset = false;
        }
        if (Input.GetKeyDown(GameManager.keybind["Dash"])) //Checks whether or not the player is attempting to dash.
        {
            if (canDash == true)
            {
                dash = true;
                lockMovement = true;
            }
        }
        #endregion

        #region Jump
        isGrounded = Physics2D.OverlapBox(feetPos.position, checkRadius, 0, isWalkable); //Checks for if the player is grounded or not based on a small overlap box's collider.
        if (isGrounded == true && Input.GetKeyDown(GameManager.keybind["Jump"])) //Checks if the player is grounded and space has been pressed - light jump.
        {
            rigid.velocity = Vector2.up * ySpeed * yLimiter;
            aTTimer = airTime;
            isJumping = true;
            Instantiate(jumpDust, player.transform.position + Vector3.down, Quaternion.identity);
        }
        if (Input.GetKey(GameManager.keybind["Jump"]) && isJumping == true) //Checks if space has been pressed and that the player is in the air.
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
        if (Input.GetKeyUp(GameManager.keybind["Jump"])) //Makes sure jumping isn't active when space isn't pressed.
        {
            Vector2 jX = rigid.velocity;
            if (rigid.velocity.y >= yLimiter) //Checks if the velocity is creater than the lowest value for the jump.
            {
                rigid.velocity = new Vector2(jX.x, 0);
                isJumping = false;
            }

        }
        #endregion

    }
    #endregion

    #region Locks
    void Locks()
    {
        switch (lockState)
        {
            case LockState.lockAll: //Used to lock all functions.
                lockMovement = true;
                lockAbilities = true;
                rigid.velocity = new Vector3(0, rigid.velocity.y);
                lockState = LockState.decease;
                break;
            case LockState.unlockAll: //Used to unlock all functions.
                lockMovement = false;
                lockAbilities = false;
                lockState = LockState.decease;
                break;
        }

        #region Fall time
        if (isGrounded == false) //Used to count fall distance as time.
        {
            fallTime += Time.deltaTime;
        }
        else if (isGrounded == true && fallTime >= 1.5f) //Checks whether or not the player is grounded once again and that the fall time was greater than 1.5 seconds; Locks the player into place if so and plays an animation.
        {
            lockState = LockState.lockAll;
            landTime++;
            //Debug.Log("Land Time: " + landTime);
            GetComponent<Player>().anim.SetBool("tooHigh", true);
            if (landTime >= 40) //Unlocks the player's movement and ends the animation.
            {
                GetComponent<Player>().anim.SetBool("tooHigh", false); //Gonna be real, don't know why this is under Locks. Might move it later.
                lockState = LockState.unlockAll;
                Debug.Log("locked: " + lockMovement);
                fallTime = 0;
                landTime = 0;
            }
        }
        else //Resets the fall time back to 0.
        {
            fallTime = 0;
        }
        #endregion
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
                canDash = false;
                dashAvaliable = 0;
                dashTimer -= Time.deltaTime;
                if (isFacing == true) //Checks what direction the dash is being activated from and acts accordingly.
                {
                    force = 1f;
                }
                else
                {
                    force = -1f;
                }
                rigid.velocity = new Vector2(force * xSpeed * accelSpeed, rigid.velocity.y);
            }
            else //Disables Dash. 
            {
                unlockYAxis = true; //disable y axis lock here.
                fallTime = 0;
                dashTimer = dashTimeReset;
                lockMovement = false;
                dashCooldown = true;
                dash = false;
            }
        }

        if (lockYAxis == true) //Used to lock the YAxis while the player is dashing.
        {
            //tempGravity = Physics2D.gravity;
            Physics2D.gravity = Vector2.zero;
            //Debug.Log("Gravity: " + Physics2D.gravity);
            tempYVelocity = rigid.velocity;
            rigid.velocity = new Vector2(tempYVelocity.x, 0);
            lockYAxis = false;
        }
        if (unlockYAxis == true) //Used to restore the values lost when locking the YAxis (Yes, it unlocks the YAxis).
        {
            //Debug.Log("Gravity before unlock: " + Physics2D.gravity);
            Physics2D.gravity = tempGravity;
            rigid.velocity = tempYVelocity;
            unlockYAxis = false;
        }
    }
    #endregion

    #region Knockback
    public void Knockback()
    {
        if (kBTimer > 0) //Checks if the knockback timer has been reset.
        {
            kBTimer -= Time.deltaTime;
            lockMovement = true;
            if (enemyPos.position.x > player.transform.position.x)
            {
                knockbackDirection = false;
            }
            else if (enemyPos.position.x < player.transform.position.x)
            {
                knockbackDirection = true;
            }

            if (knockbackDirection == true) //Applies knockback based on the direction the player was hit from.
            {
                rigid.velocity = new Vector2(knockback.x, knockback.y);
            }
            else
            {
                rigid.velocity = new Vector2(-knockback.x, knockback.y);
            }
        }
        else //Resets the ability to take knockback.
        {
            lockMovement = false;
            kBTimer = knockbackTime;
            enemyPos = null;
            beenKnocked = false;
        }
    }
    #endregion

    #region Movement - Fixed
    public void MovementF() //Fixed Movement - Allows for horizontal input and movement.
    {
        if (Input.GetKey(GameManager.keybind["Left"]))
        {
            force = -1;
        }
        if (Input.GetKey(GameManager.keybind["Right"]))
        {
            force = 1;
        }
        if (!Input.GetKey(GameManager.keybind["Right"]) && !Input.GetKey(GameManager.keybind["Left"]) || Input.GetKey(GameManager.keybind["Right"]) && Input.GetKey(GameManager.keybind["Left"]))
        {
            force = 0;  
        }

        rigid.velocity = new Vector2(force * xSpeed, rigid.velocity.y);

        if (force != 0)
        {
            //Used for spawning walking effect.
        }
    }
    #endregion
}

public enum LockState //Used to unlock and lock abilities.
{ 
    lockAll,
    unlockAll,
    decease
}