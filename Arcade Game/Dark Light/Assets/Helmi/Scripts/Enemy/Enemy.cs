//==========================================
// Title:  Dark Light
// Author: Helmi Amsani
// Date:   14 May 2019
//==========================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("What type of enemy is this?")]
    public bool groundEnemy;
    public bool flyingEnemy;

    [Header("Attack Stuff")]
    public float attackDelay;
    private float lastAttackTime;
    public float attackRange = 1.5f;
    public Transform attackDetector;

    [Header("Base Enemy Attributes")]    
    public State currentState;
    public float initialSpeed = 5f;
    private float _speed;

    [Header("Patrol & Chasing Attributes")]
    public float playerDetectorDistance = 15f;
    [HideInInspector]
    public bool moveRight = true;

    [Header("Ground Enemy Detector")]
    public float groundDetectorDistance = 2f;
    public Transform groundDetector;

    [Header("Flying Enemy Detector")]
    public Transform originPlace;
    public float stoppingDistance = 9f;
    public float retreatDistance = 8f;

    [Header("All Player Attributes")]
    public GameObject player;

    //Reference
    private Player playerScrpt;

    // Testing
    [Header("Animation Testing")]
    public Animator anim;
    #endregion

    #region Start
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerScrpt = player.GetComponent<Player>();
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
    public void SetDirection(Vector3 direction, bool isMovingRight)
    {
        // Rotates enemy back to original 
        transform.eulerAngles = direction;
        // Now enemy moves right
        moveRight = isMovingRight;
    }
    #endregion

    #region Base Enemy Movement
    void BaseEnemyMovement()
    {
        #region Ground Enemy
        // When it is a ground enemy
        if (groundEnemy == true)
        {
            //========================
            // Referenced from this website: https://www.youtube.com/watch?v=aRxuKoJH9Y0
            // Ground dectetor detects ground collider
            RaycastHit2D groundHit = Physics2D.Raycast(groundDetector.position, Vector2.down, groundDetectorDistance);
            Debug.DrawRay(groundDetector.position, Vector2.down, Color.red);

            if (groundHit.collider == true )
            {
                // Moves enemy to right
                transform.Translate(Vector2.right * _speed * Time.deltaTime);
            }
            //=========================

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
        #endregion

        #region Flying Enemy
        if (flyingEnemy == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, originPlace.position, _speed * Time.deltaTime);
        }
        #endregion
    }
    #endregion

    #region Seeking Player
    void SeekPlayer()
    {
        #region Ground Enemy
        if (groundEnemy == true)
        {
            Vector2 seekPosition = player.transform.position;
            seekPosition.y = transform.position.y;
            transform.position = Vector2.MoveTowards(transform.position, seekPosition, _speed * Time.deltaTime);
        }
        #endregion

        #region Flying Enemy
        if (flyingEnemy == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);
        }
        #endregion

        // Player is on the left side of Enemy
        if (player.transform.position.x < transform.position.x)
        {
            // Flip the enemy to left
            SetDirection(new Vector3(0, -180, 0), false);
        }
        // Player is on the right side of Enemy
        else
        {
            // Flip the enemy to right
            SetDirection(Vector3.zero, true);
        }
    }
    #endregion

    #region When Player Inside any circle radius
    void PlayerInsideRange()
    {        
        // the distance between enemy and player
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        #region Chase player or not
        // Player inside the red Circle Range
        if (distanceToPlayer <= playerDetectorDistance)
        {
            #region Ground Enemy
            if (groundEnemy == true)
            {
                currentState = State.Seek;
                SeekPlayer();
            }
            #endregion

            #region Flying Enemy
            if (flyingEnemy == true)
            {
                if (distanceToPlayer <= stoppingDistance && retreatDistance <= distanceToPlayer)
                { }
                else if (distanceToPlayer <= retreatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -(_speed * 0.75f) * Time.deltaTime);
                }
                else
                {
                    currentState = State.Seek;
                    SeekPlayer();
                }
            }
            #endregion
        }
        // Player outside the red Circle Range
        else
        {
            currentState = State.Patrol;
            BaseEnemyMovement();
        }
        #endregion

        #region Enemy stops when close to player or not

        #region Ground Enemy
        if (groundEnemy == true)
        {
            // If player inside the hit box range
            if (distanceToPlayer <= attackRange)
            {
                currentState = State.hit;
                _speed = 0;
                // Damage to player
                DamagePlayer();
            }
            // If it is not
            else
            {
                currentState = State.Patrol;
                _speed = initialSpeed;
                anim.SetBool("IsAttack", false);
            }
        }
        #endregion

        #region Flying Enemy
        if(flyingEnemy == true)
        {

        }
        #endregion

        #endregion
    }
    #endregion

    #region Damage To Player
    void DamagePlayer()
    {
        #region Ground Enemy
        if(groundEnemy == true)
        {
            //==============
            // Referenced from this website: https://www.youtube.com/watch?v=LqCJowEQFBc
            if (Time.time > lastAttackTime + attackDelay)
            {
                playerScrpt.beenHit = true;
                anim.SetBool("IsAttack", true);
                lastAttackTime = Time.time;
            }
            //============== 
        }
        #endregion

        #region Flying Enemy
        if (flyingEnemy == true)
        {

        }
        #endregion
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
            Gizmos.DrawWireSphere(attackDetector.position, attackRange);
        }
        if(flyingEnemy == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, retreatDistance);
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
