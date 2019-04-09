using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("Player Script")]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    //Mechanics 
    private int accel = 3; //default value for dash.
    private int health = 6; //default value got player health.
    private int damage = 1; //temp, may be moved to child class (sword/weapon).
    private float xSpeed; //default value for movement on the x-axis.
    private float ySpeed; //default value for movement on the y-axis.
    private float gravity; //default value of gravity for player.
    private bool iFrame = false; //tested for whether or not Dash has been given an iFrame.
    private bool dash = false; //activates and locks when dash hotkey is pressed.
    private bool attack = false; //activates and locks when attack hotkey is pressed.
    private bool beenHit = false; //activates and locks to give an additional iFrame for a brief moment after the player has been hit.
    private bool facing = true; //true = right, false = left. Changes depending on what face the player is changing.
    //Counters
    private int iFCounter = 0; //counter for iFrame activation.
    private int aCounter = 0; //counter for attack activation.
    private int dCounter = 0; //counter for dash activation.

    public void Start() 
	{
	}
    public void FixedUpdate() //basic update cycle.
    {
        Movement();
        Attack();
        Dash();
        IFrame();
        Health();
    }
    //[Header("Movement")]
    public void Movement() //deals with input for movement.
    {
        if (Input.GetKeyDown("w"))
        {

        }
        if (Input.GetKeyDown("a"))
        {

        }
        if (Input.GetKeyDown("s"))
        {

        }
        if (Input.GetKeyDown("d"))
        {

        }
    }
    //[Header("Attack")]
    public void Attack() //deals with attack activation sequence.
    {
        if (Input.GetKeyDown("e"))
        {
            //boss.Heath = Boss.Health - Damage; (boss doesn't exist currently)
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
