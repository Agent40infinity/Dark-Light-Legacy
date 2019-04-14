using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Player Script")]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    //Mechanics:
    private int accel = 3; //default value for dash.
    private int health = 6; //default value got player health.
    private int damage = 1; //temp, may be moved to child class (sword/weapon).
    private bool iFrame = false; //tested for whether or not Dash has been given an iFrame.
    private bool dash = false; //activates and locks when dash hotkey is pressed.
    private bool attack = false; //activates and locks when attack hotkey is pressed.
    private bool beenHit = false; //activates and locks to give an additional iFrame for a brief moment after the player has been hit.
    private bool facing = true; //true = right, false = left. Changes depending on what face the player is changing.

    //Attacking:
    private float attackCooldown;
    private float startACooldown;
    public float attackRange;

    //Counters:
    private int iFCounter = 0; //counter for iFrame activation.
    private int aCounter = 0; //counter for attack activation.
    private int dCounter = 0; //counter for dash activation.

    //Reference:
    public Transform attackPos;
    public LayerMask isEnemy;

    public void Start() 
	{
	}
    public void FixedUpdate() //basic update cycle.
    {
        Attack();
        Dash();
        IFrame();
        Health();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    //[Header("Attack")]
    public void Attack() //deals with attack activation sequence.
    {
        if (attackCooldown <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
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
    //[Header("Dash")]
    public void Dash() //deals with the activation of dash.
    {
        if (Input.GetKeyDown("left shift"))
        {
            if (facing == true)
            {
                iFrame = true;
                //vector2, required for movement, learn this. 
            }
            else
            {
                iFrame = true;
                //vector2, required for movement, learn this. 
            }
        }
    }
    public void IFrame() //deals with the activation of the iFrame after certain activations.
    {
        if (iFrame == true)
        {
            iFCounter++;
            if (iFCounter <= 30*Time.deltaTime)
            {
                iFCounter = 0;
                iFrame = false;
            }
        }
    }
    //[Header("Health")]
    public void Health() //deals with health deduction and external UI changes.
    {
        if (beenHit == true && iFrame == false && health >= 1)
        {
            health--;
            iFrame = true;
            beenHit = false;
        }
        if (health == 0)
        {
            //UI.Gameover = true;
        }
    }
}
