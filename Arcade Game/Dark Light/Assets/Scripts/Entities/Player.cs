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
    public static bool isDead = false;
    bool isFadingFromDeath = false;

    //Attacking:
    private int attackCooldown;
    private int startACooldown = 15;
    public float attackRange;

    //Counters:
    private int iFCounter = 0; //counter for iFrame activation.
    private int aCounter = 0; //counter for attack activation.
    private int dCounter = 0; //counter for dash activation.
    private int hCounter = 0;
    private float dthCounter = 0;
    private float fICounter = 0;
    private float fOCounter = 0;

    //Reference:
    public GameObject death;
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
        Debug.Log("AttackCooldown" + attackCooldown);
        if (attackCooldown <= 0)
        {
            if (Input.GetButtonDown("Attack"))
            {
                anim.SetBool("Attack_Down", true);
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);
                for (int i = 0; i < enemiesInRange.Length; i++)
                {
                    //enemiesInRange[i].GetComponent<EnemyHealth>.TakeDamage(damage);     
                    //boss.Heath = Boss.Health - Damage; //(boss doesn't exist currently)
                }
                attackCooldown = startACooldown;
            }   
        }
        else
        {
            anim.SetBool("Attack_Down", false);
            attackCooldown--;
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
        anim.SetBool("Death", isDead);
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
                print("fadeOut");
                //if (hCounter >= 6)
                //{
                    hCounter = 0;
                    transform.position = FallCheckpoint.cPos[FallCheckpoint.lastPassed].position;
                print("fadeIn");

                fade.GetComponent<FadeController>().FadeIn();
                    hitHostile = false;
                //}
            }
            else
            {
                hitHostile = false;
            }
        }


        if (curHealth <= 0 && !isFadingFromDeath)
        {
            StartCoroutine("_Die");
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
                {
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
    IEnumerator _Die()
    {
        isFadingFromDeath = true;
        player.GetComponent<PlayerMovement>().lockMovement = true;
        beenHit = false;
        isDead = true;

        yield return new WaitForSeconds(2.5f);
        fade.GetComponent<FadeController>().FadeOut();

        yield return new WaitForSeconds(1f);
        death.SetActive(true);
        Instantiate(darkLight, transform.position, transform.rotation);
        transform.position = Lamp.lPos[Lamp.lastSaved].position;
        curHealth = maxHealth;
        isDead = false;

        yield return new WaitForSeconds(5f);
        death.SetActive(false);
        fade.GetComponent<FadeController>().FadeIn();
        isFadingFromDeath = false;

        yield return new WaitForSeconds(2f);
        player.GetComponent<PlayerMovement>().lockMovement = false;
    }
}
