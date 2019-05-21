using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Base Enemy Attributes")]

    public State currentState;
    public float initialSpeed = 5f;
    private float _speed;

    [Header("Patrol & Chasing Attributes")]

    public float groundDetectorDistance = 2f;
    public float playerDetectorDistance = 5f;   
    private bool moveRight = true;
    public Transform groundDetector;
    public Transform playerDetector;

    [Header("Enemy Hitbox Attributes")]

    public float hitBoxRange = 1.5f;
    public Transform hitDetector;

    [Header("All Player Attributes")]

    public Transform player;
    public Transform chaseDot;
    #endregion

    #region Start
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        groundDetector = GameObject.Find("GroundDetector").transform;
        playerDetector = GameObject.Find("PlayerDetector").transform;
        hitDetector = GameObject.Find("HitDetector").transform;
        chaseDot = GameObject.Find("ChaseDot").transform;
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
                BaseEnemyMovement();
                break;
            case State.Seek:
                SeekPlayer();
                break;
            case State.hit:
                break;
            default:
                BaseEnemyMovement();
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
        float speedSeek = (_speed / _speed) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, chaseDot.position, speedSeek);
        BaseEnemyMovement();
    }
    #endregion

    #region When Player Inside any ranges
    void PlayerInsideRange()
    {
        #region Chase player or not
        RaycastHit2D playerHit = Physics2D.Raycast(playerDetector.position, Vector2.right, playerDetectorDistance);

        if (playerHit.collider == true)
        {
            currentState = State.Seek;
        }
        else
        {
            currentState = State.Patrol;
        }
        #endregion

        #region Enemy stops when close to player or not
        // the distance between enemy and player
        float distanceToPlayer = Vector2.Distance(hitDetector.position, player.position);

        // If player inside the hit box range
        if(distanceToPlayer <= hitBoxRange)
        {
            currentState = State.hit;
            _speed = 0;
            Damage();
        }
        // If it is not
        else
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
