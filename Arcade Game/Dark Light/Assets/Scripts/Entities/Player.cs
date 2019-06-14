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
    //Mechanics:
    public static int curHealth; //Value for player's current health.
    public static int maxHealth = 5; //default value for player's max health.
    public static int maxWisps = 3; //Max value of how many Wisps the player can have.
    public static int curWisps; //Max value of how many Wisps the player can have.
    private int damage = 1; //temp, may be moved to child class (sword/weapon).
    public bool iFrame = false; //tested for whether or not Dash has been given an iFrame.
    private bool attack = false; //activates and locks when attack hotkey is pressed.
    public bool beenHit = false; //activates and locks to give an additional iFrame for a brief moment after the player has been hit.
    public bool hitHostile = false;
    public static bool isDead = true;

    //Attacking:
    private float attackCooldown;
    private float startACooldown;
    public float attackRange;

    //Counters:
    private int iFCounter = 0; //counter for iFrame activation.
    private int aCounter = 0; //counter for attack activation.
    private int dCounter = 0; //counter for dash activation.
    private int hCounter = 0;

    //Reference:
    public GameObject player;
    public GameObject darkLight;
    public GameObject fade;
    public Transform attackPos;
    public LayerMask isEnemy;
    public Animator anim;
    public SpriteRenderer rend;

    int GetNumberFromString(string word)
    {
        string number = Regex.Match(word, @"\d+").Value;

        int result;
        if (int.TryParse(number, out result))
        {
            return result;
        }

        return -1;
    }

    public void Start() 
	{
        curHealth = maxHealth;
        curWisps = maxWisps;
	}

    public void Update()
    {
        Health();
        if (Input.GetKeyDown(KeyCode.P))
        {
            curHealth--;
            Debug.Log("Health: " + curHealth);
        }
    }
    public void FixedUpdate() //basic update cycle.
    {
        Attack();
        IFrame();
        FaceCheck();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.tag == "Checkpoint")
        {
            int pos = GetNumberFromString(other.name);
            if (pos > 0 && pos < FallCheckpoint.cPos.Length)
            {
                FallCheckpoint.lastPassed = pos;
            }
            Debug.Log("Updated lastPassed: " + FallCheckpoint.lastPassed);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.tag == "Save")
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                int pos = GetNumberFromString(other.name);
                if (pos > 0 && pos < Lamp.lPos.Length)
                {
                    Lamp.lastSaved = pos;
                }
                Debug.Log("Updated lastSaved: " + Lamp.lastSaved);
            }
        }
        if (other.tag == "HostileEnvironment")
        {
            hitHostile = true;            
        }
    }

    public void FaceCheck()
    {
        if ((int)Input.GetAxisRaw("Vertical") == 1)
        {
            attackPos.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.5f);
        }
        else if ((int)Input.GetAxisRaw("Vertical") == -1 && player.GetComponent<PlayerMovement>().isGrounded == false)
        {
            attackPos.position = new Vector2(player.transform.position.x, player.transform.position.y - 1.5f);
        }
        else if (player.GetComponent<PlayerMovement>().isFacing == true)
        {
            attackPos.position = new Vector2(player.transform.position.x + 1, player.transform.position.y);
        }
        else
        {
            attackPos.position = new Vector2(player.transform.position.x - 1, player.transform.position.y);
        }
    }

    public void Attack() //deals with attack activation sequence.
    {
        if (attackCooldown <= 0)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    //enemiesInRange[i].GetComponent<Enemy>.TakeDamage(damage);     
                    //boss.Heath = Boss.Health - Damage; (boss doesn't exist currently)
                }
            }
            attackCooldown = startACooldown;
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown("e"))
        {
            
        }
    }

    public void IFrame() //deals with the activation of the iFrame after certain activations.
    {
        if (iFrame == true)
        {
            iFCounter++;
            Debug.Log("iFCounter: " + iFCounter);
            if (iFCounter >= 60)
            {
                iFCounter = 0;
                iFrame = false;
                Debug.Log("Read iFrame: " + iFrame);
            }
        }
    }
    //[Header("Health")]
    public void Health() //deals with health deduction and external UI changes.
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            beenHit = true;
        }

        if (beenHit == true && iFrame == false && curHealth >= 1)
        {
            curHealth--;
            if (curHealth != 0)
            {
                player.GetComponent<PlayerMovement>().beenKnocked = true;
                iFrame = true; 
            }
            beenHit = false;
        }

        if (hitHostile == true)
        {
            curHealth--;
            if (curHealth != 0)
            {
                //hCounter++;
                player.GetComponent<PlayerMovement>().beenKnocked = true;
                fade.GetComponent<FadeController>().FadeOut();
                //if (hCounter >= 6)
                //{
                    hCounter = 0;
                    transform.position = FallCheckpoint.cPos[FallCheckpoint.lastPassed].position;
                    fade.GetComponent<FadeController>().FadeIn();
                    hitHostile = false;
                //}
            }
            else
            {
                hitHostile = false;
            }
        }

        if (curHealth <= 0)
        {
            fade.GetComponent<FadeController>().FadeOut();
            Instantiate(darkLight, transform.position, transform.rotation);
            isDead = true;
            transform.position = Lamp.lPos[Lamp.lastSaved].position;
            curHealth = maxHealth;
            fade.GetComponent<FadeController>().FadeIn();
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
}
