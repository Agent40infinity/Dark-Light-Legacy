﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("What type of enemy is this?")]

    public bool groundEnemy;
    public bool flyingEnemy;


    [Header("Base Enemy Attributes")]
    
    public State currentState;
    public float initialSpeed = 5f;
    private float _speed;
    private SpriteRenderer rend;

    [Header("Patrol & Chasing Attributes")]

    public float detectorDistance = 2f;
    public float playerDetectorDistance = 2f;   
    private bool moveRight = true;
    public Transform groundDetector;
    public Transform RWallDetector;
    public Transform LWallDetector;

    [Header("Enemy Hitbox Attributes")]

    public float hitBoxRange = 1.5f;
    public Transform hitDetector;

    [Header("All Player Attributes")]

    public Transform player;
    #endregion

    #region Start
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rend = GetComponent<SpriteRenderer>();
        if(groundEnemy == true)
        {
            groundDetector = GameObject.Find("GroundDetector").transform;
            RWallDetector = GameObject.Find("RightWallDetector").transform;
            LWallDetector = GameObject.Find("LeftWallDetector").transform;
            hitDetector = GameObject.Find("HitDetector").transform;
        }
        _speed = initialSpeed;
        currentState = State.Patrol;
    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                break;
            case State.Seek:
                break;
            case State.hit:
                break;
        }

        PlayerInsideRange();
    }
    #endregion

    #region Set Direction
    void SetDirection(Vector3 direction, bool isMovingRight)
    {
        // Rotates enemy back to original 
        transform.eulerAngles = direction;
        // Now enemy moves right
        moveRight = isMovingRight;
    }
    #endregion

    #region Cool Dude
    /*
    void RayDetector(RaycastHit2D detectorRay)
    {
        detectorRay = Physics2D.Raycast(detector.position, Vector2.down, detectorDistance);
        Debug.DrawRay(detector.position, Vector2.down, Color.red);
    }
    */
    #endregion

    #region Base Enemy Movement
    void BaseEnemyMovement()
    {
        if(groundEnemy == true)
        {
            // Ground dectetor detects ground collider
            RaycastHit2D groundHit = Physics2D.Raycast(groundDetector.position, Vector2.down, detectorDistance);
            Debug.DrawRay(groundDetector.position, Vector2.down, Color.red);

            RaycastHit2D RWallHit = Physics2D.Raycast(RWallDetector.position, Vector2.right, detectorDistance);
            Debug.DrawRay(RWallDetector.position, Vector2.right, Color.red);

            RaycastHit2D LWallHit = Physics2D.Raycast(LWallDetector.position, Vector2.left, detectorDistance);
            Debug.DrawRay(LWallDetector.position, Vector2.left, Color.red);

            // When there is ground collider, the ground detector wil do this 
            if (groundHit.collider == true )
            {
                // Moves enemy to right
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }

            else if(RWallHit.collider == true)
            {
                // Moves enemy to right
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }

            else if (LWallHit.collider == true)
            {
                // Moves enemy to right
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }

            // When there is no ground collider, the ground detector will do this
            else
            {
                // When enemy moves right
                if (moveRight == true)
                {
                    SetDirection(new Vector3(0, -180, 0), false);
                }

                // When enemy moves left
                else
                {
                    SetDirection(Vector3.zero, true);
                }
            }
        }

        if(flyingEnemy == true)
        {

        }

    }
    #endregion

    #region Seeking Player
    void SeekPlayer()
    {
        Vector2 seekPosition = player.position;
        seekPosition.y = transform.position.y;

        if (groundEnemy == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, seekPosition, _speed * Time.deltaTime);
        }
        
        if(flyingEnemy == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, _speed * Time.deltaTime);
        }

        if(player.position.x < transform.position.x)
        {
            SetDirection(new Vector3(0, -180, 0), false);
        }
        else
        {
            SetDirection(Vector3.zero, true);
        }
    }
    #endregion

    #region When Player Inside any ranges
    void PlayerInsideRange()
    {

        // the distance between enemy and player
        float distanceToPlayer = Vector2.Distance(hitDetector.position, player.position);

        #region Chase player or not
        // Player inside the red Circle Range
        if (distanceToPlayer <= playerDetectorDistance)
        {
            currentState = State.Seek;
            SeekPlayer();
        }
        // Player outside the red Circle Range
        else if (distanceToPlayer > playerDetectorDistance)
        {
            currentState = State.Patrol;
            BaseEnemyMovement();
        }
        #endregion

        #region Enemy stops when close to player or not

        if(groundEnemy == true)
        {
            // If player inside the hit box range
            if (distanceToPlayer <= hitBoxRange)
            {
                currentState = State.hit;
                _speed = 0;
                Damage();
            }
            // If it is not
            else if (distanceToPlayer > hitBoxRange)
            {
                currentState = State.Patrol;
                _speed = initialSpeed;
            }
        }
        #endregion
    }
    #endregion

    #region Damage
    void Damage()
    {

    }
    #endregion

    #region Bunch of Art stuff
    // Don't how to draw a line so I'll use Sphere XD
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectorDistance);

        if(groundEnemy == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(hitDetector.position, hitBoxRange);
        }
    }
    #endregion

    public enum State
    {
        Patrol,
        Seek,
        hit
    }
}
