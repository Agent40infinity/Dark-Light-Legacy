using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/*---------------------------------/
 * Script created by Aiden Nathan.
 *---------------------------------*/

[AddComponentMenu("Player Script")]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    #region Variables
    //Mechanics:
    public static int curHealth; //Value for player's current health.
    public static int maxHealth = 5; //default value for player's max health.
    public static int maxWisps = 3; //Max value of how many Wisps the player can have.
    public static int curWisps; //Max value of how many Wisps the player can have.
    private int damage = 2; //temp, may be moved to child class (sword/weapon).
    public bool iFrame = false; //tested for whether or not Dash has been given an iFrame.
    private bool attack = false; //activates and locks when attack hotkey is pressed.
    public bool beenHit = false; //activates and locks to give an additional iFrame for a brief moment after the player has been hit.
    public bool hitHostile = false; //Checks whether or not the player has hit a hostile environment object.
    public static bool isDead = false; //Checks whether or not the player has died.
    public bool takenHealth = false;
    public static bool recovered = false;
    bool fadeIntoDeath = false; //Used to call upon the Sub-routine "Death".

    //Attacking:
    private int attackCooldown; //Cooldown for attacking.
    private int startACooldown = 15; //Used to reset the attack cooldown.
    public float attackRange; //Distance used to determine the radius of the attack range.

    //Counters:
    private int iFCounter = 0; //Counter for iFrame activation.
    private int aCounter = 0; //Counter for attack activation.
    private int dCounter = 0; //Counter for dash activation.
    private float hCounter = 0; //Counter for hostile activation.
    //private float dthCounter = 0; //Counter for death screen activation.
    //private float fICounter = 0; //Counter for Fade-In activation.
    //private float fOCounter = 0; //Counter for Fade-Out activation.

    //Reference:
    public GameObject death; //Reference for the death screen.
    public GameObject player; //Reference for the player itself.
    public GameObject darkLight; //Reference for the DarkLight (dropped upon death collectable soul).
    public GameObject fade; //Reference for the Fade_to_Black screen.
    public Transform attackPos; //References the transform of the attak gameobject tied to player.
    public LayerMask isEnemy; //Mask to check whether or not an object is an enemy.
    public Animator anim; //Reference for the animator attached to player.
    public SpriteRenderer rend; //Reference for the sprite renderer attached to player.
    public GameObject save;

    int GetNumberFromString(string word) //Allows for the trasnlation of strings into integers.
    {
        string number = Regex.Match(word, @"\d+").Value;

        int result;
        if (int.TryParse(number, out result))
        {
            return result;
        }
        return -1;
    }
    #endregion

    #region General
    public void Start()
    {
        curHealth = maxHealth;
        curWisps = maxWisps;
    }

    public void Update()
    {
        Health();
        if (player.GetComponent<PlayerMovement>().lockAbilities == false && player.GetComponent<PlayerMovement>().dash == false)
        {
            Attack();
        }
    }
    public void FixedUpdate() //basic update cycle.
    {
        IFrame();
        FaceCheck();
    }

    void OnDrawGizmosSelected() //Used to visualise positioning of hidden objects within the scene.
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void OnTriggerEnter2D(Collider2D other) //Used to check if the player has passed a checkpoint.
    {
        Debug.Log(other);
        if (other.tag == "Checkpoint") //Looks for the tag attached to each checkpoint.
        {
            int pos = GetNumberFromString(other.name);
            if (pos > 0 && pos < FallCheckpoint.cPos.Length) //Checks for the number within the name of the GameObject and links it to the index for the transforms stored in the array.
            {
                FallCheckpoint.lastPassed = pos;
            }
            Debug.Log("Updated lastPassed: " + FallCheckpoint.lastPassed);
        }
    }

    void OnTriggerStay2D(Collider2D other) //Checks whether or not the player can interact with the save points.
    {
        Debug.Log(other);
        if (other.tag == "Save") //Checks for the tag attached to each save point.
        {
            if (Input.GetKeyDown(KeyCode.F)) //Checks if the player is wanting to interact with the save point.
            {
                int pos = GetNumberFromString(other.name);
                if (pos > 0 && pos < Lamp.lPos.Length) //Checks if the number from the GameObject has been stored previously in the array of checkpoints and index's it.
                {
                    Lamp.lastSaved = pos;
                    other.gameObject.GetComponent<LampController>().LightLamp();
                    save.GetComponent<Animator>().SetTrigger("SaveLoad");
                }
                Debug.Log("Updated lastSaved: " + Lamp.lastSaved);
            }
        }
        if (other.tag == "HostileEnvironment") //Checks if the player has touched a hostile environment object.
        {
            hitHostile = true;
        }
    }
    #endregion

    #region Attacking
    public void FaceCheck() //Used to determine where the hitbox for attacking will be placed.
    {
        if ((int)Input.GetAxisRaw("Vertical") == 1) //Checks if the player is looking up.
        {
            attackPos.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        }
        else if ((int)Input.GetAxisRaw("Vertical") == -1 && player.GetComponent<PlayerMovement>().isGrounded == false) //Checks if the player is looking down and is currently not on the ground.
        {
            attackPos.position = new Vector2(player.transform.position.x, player.transform.position.y - 1.5f);
        }
        else if (player.GetComponent<PlayerMovement>().isFacing == true) //Checks if the player is facing right.
        {
            attackPos.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
        }
        else //Defaults to facing left is nothing else returns true.
        {
            attackPos.position = new Vector2(player.transform.position.x - 1, player.transform.position.y);
        }
    }

    public void Attack() //deals with attack activation sequence.
    {
        Debug.Log("AttackCooldown" + attackCooldown);
        if (attackCooldown <= 0) //Checks if the attack is off cooldown.
        {
            if (Input.GetButtonDown("Attack")) //Checks if the player is attempting to attack via keypress.
            {
                anim.SetBool("Attack_Down", true);
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);
                for (int i = 0; i < enemiesInRange.Length; i++) //Deals damage to all enemies within the radius of the attack hitbox.
                {
                    enemiesInRange[i].GetComponent<EnemyHealth>().TakeDamage(damage);     
                }
                attackCooldown = startACooldown;
            }
        }
        else //Used to make sure the player can attack again after cooldown and allow for other animations.
        {
            anim.SetBool("Attack_Down", false);
            attackCooldown--;
        }
    }
    #endregion

    #region iFrame Controller
    public void IFrame() //deals with the iFrame controller after the activation of certain abilities and actions.
    {
        if (iFrame == true) //Checks if iFrame has been set to true.
        {
            iFCounter++;
            Debug.Log("iFCounter: " + iFCounter);
            if (iFCounter >= 60) //Checks that enough time has passed so the iFrame can end and allow the player to take damage again.
            {
                iFCounter = 0;
                iFrame = false;
                Debug.Log("Read iFrame: " + iFrame);
            }
        }
    }
    #endregion

    #region Health Management
    public void Health() //deals with health deduction and external UI changes.
    {
        anim.SetBool("Death", isDead);
        if (Input.GetKeyDown(KeyCode.O))
        {
            curHealth = 0;
        }

        if (beenHit == true && iFrame == false && curHealth >= 1) //Checks whether or not the player has been hit when their health is above 1 while there is no iFrame activated.
        {
            curHealth--;
            if (curHealth != 0) //Applies knockback to the player only while their health isn't 0.
            {
                player.GetComponent<PlayerMovement>().beenKnocked = true;
                iFrame = true;
            }
            beenHit = false;
        }

        if (hitHostile == true) //Checks if the player has collided with a hostile environment object.
        {
            curHealth--;
            if (curHealth != 0) //Checks whether or not the player's health isn't 0; Then applies Knockback it isn't 0, starts a fade out, teleports the player away from danger (to the nearest safe space), and then proceeds to fade back in.
            {
                //hCounter += Time.deltaTime;
                //player.GetComponent<PlayerMovement>().beenKnocked = true;
                fade.GetComponent<FadeController>().FadeOut();
                //if (hCounter >= 0.16)
                //{
                    player.GetComponent<PlayerMovement>().rigid.velocity = new Vector2(0, 0);
                    transform.position = FallCheckpoint.cPos[FallCheckpoint.lastPassed].position;
                    player.GetComponent<PlayerMovement>().fallTime = 2f;
                    iFrame = true;
                    fade.GetComponent<FadeController>().FadeIn();
                    hCounter = 0;
                    hitHostile = false;
                //}
            }
            else
            {
                hitHostile = false;
            }
        }

        if (curHealth <= 0 && !fadeIntoDeath) //Checks whether or not the curret health is less than or equal to 0 and that fadeIntoDeath has not already been activated.
        {
            player.GetComponent<PlayerMovement>().lockAll = true; //Locks all movement, actions, and abilities.
            StartCoroutine("Death");
            /*
            beenHit = false;
            isDead = true;
            dthCounter += Time.deltaTime;
            //Debug.Log("Death Counter: " + dthCounter);
            if (dthCounter >= 2.50f)
            {
                fICounter += Time.deltaTime;
                fade.GetComponent<FadeController>().FadeOut();
                print("fadeOut");

                Debug.Log("!KillMe");
                if (fICounter >= 1f)
                {                                                                                 Old Broken Code: Replaced by Co-routine / Enum.
                    fOCounter += Time.deltaTime;
                    death.SetActive(true);
                    isDead = false;
                    if (fOCounter >= 5f)
                    {
                        fade.GetComponent<FadeController>().FadeIn();
                        print("fadeIn");

                        death.SetActive(false);
                        Instantiate(darkLight, transform.position, transform.rotation);
                        transform.position = Lamp.lPos[Lamp.lastSaved].position;
                        curHealth = maxHealth;
                        dthCounter = 0;
                        fICounter = 0;
                        fOCounter = 0;
                    }
                }
            }
            */
        }

        if (isDead == true)
        {
            maxWisps = 0;
        }
        else
        {
            maxWisps = 3;
        }
    }

    #region Death
    IEnumerator Death() //Called upon to show that the player has died; Makes the player un-hittable and dead.
    {
        fadeIntoDeath = true;
        beenHit = false;
        isDead = true;

        yield return new WaitForSeconds(2.5f); //Fades out after death animation is played.
        fade.GetComponent<FadeController>().FadeOut();

        yield return new WaitForSeconds(1f); //Shows the death screen and plays the animation while also resetting what's needed for the player to begin playing again.
        death.SetActive(true);
        Instantiate(darkLight, transform.position, transform.rotation);
        transform.position = Lamp.lPos[Lamp.lastSaved].position;
        curHealth = maxHealth;

        isDead = false;

        yield return new WaitForSeconds(5f); //Fades back in after the animation for the death screen is complete.
        death.SetActive(false);
        fade.GetComponent<FadeController>().FadeIn();
        save.GetComponent<Animator>().SetTrigger("SaveLoad");
        fadeIntoDeath = false;

        yield return new WaitForSeconds(2f); //Unlocks all movement once the player is completely visable.
        player.GetComponent<PlayerMovement>().unlockAll = true;
        recovered = true;
    }
    #endregion
    #endregion
}
