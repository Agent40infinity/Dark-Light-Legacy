using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Base Enemy Attributes")]

    public State currentState;
    public float initialSpeed = 5f;
    public float _speed;

    [Header("Patrol & Chasing Attributes")]

    public float groundDetectorDistance = 2f;
    public float playerDetectorDistance = 3f;   
    private bool moveRight = true;
    public bool hitPlayer = false;
    public Transform groundDetector;
    public Transform playerDetector;

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
        groundDetector = GameObject.Find("GroundDetector").transform;
        playerDetector = GameObject.Find("PlayerDetector").transform;
        hitDetector = GameObject.Find("HitDetector").transform;
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

    #region Base Enemy Movement
    void BaseEnemyMovement()
    {
        // Ground dectetor detects ground collider
        RaycastHit2D groundHit = Physics2D.Raycast(groundDetector.position, Vector2.down, groundDetectorDistance);

        // When there is ground collider, the ground detector wil do this 
        if (groundHit.collider == true)
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
                // Rotates enemy 180 degrees
                transform.eulerAngles = new Vector3(0, -180, 0);
                // Now enemy moves left
                moveRight = false;
            }

            // When enemy moves left
            else
            {
                // Rotates enemy back to original 
                transform.eulerAngles = new Vector3(0, 0, 0);
                // Now enemy moves right
                moveRight = true;
            }
        }
    }
    #endregion

    #region Seeking Player
    void SeekPlayer()
    {
        hitPlayer = true;
        transform.position = Vector2.MoveTowards(transform.position, player.position, _speed * Time.deltaTime);
    }
    #endregion

    #region When Player Inside any ranges
    void PlayerInsideRange()
    {

        // the distance between enemy and player
        float distanceToPlayer = Vector2.Distance(hitDetector.position, player.position);

        #region Chase player or not
        if (distanceToPlayer <= playerDetectorDistance)
        {
            currentState = State.Seek;
            SeekPlayer();
        }
        else if (distanceToPlayer > playerDetectorDistance)
        {
            currentState = State.Patrol;
            BaseEnemyMovement();
            hitPlayer = false;
        }
        #endregion

        #region Enemy stops when close to player or not
        // If player inside the hit box range
        if(distanceToPlayer <= hitBoxRange)
        {
           currentState = State.hit;
           _speed = 0;
            Damage();
        }
        // If it is not
        else if(distanceToPlayer > hitBoxRange)
        {
            currentState = State.Patrol;
           _speed = initialSpeed;
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitDetector.position, hitBoxRange);
    }
    #endregion

    public enum State
    {
        Patrol,
        Seek,
        hit
    }
}
