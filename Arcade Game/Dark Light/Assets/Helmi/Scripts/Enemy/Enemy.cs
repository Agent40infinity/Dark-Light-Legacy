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
    // Not all variables are automatically connecting to components
    #region Variables
    [Header("What type of enemy is this?")]
    public bool groundEnemy;
    public bool flyingEnemy;

    [Header("Attack Stuff")]
    public float attackDelay;
    private float lastAttackTime;
    public float attackRange = 1.5f;
    public float bulletSpeed = 20f;
    public Transform attackDetector;

    [Header("Base Enemy Attributes")]
    public State currentState;
    public float initialSpeed = 5f;
    private float _speed;

    [Header("Patrol & Chasing Attributes")]
    public float playerDetectorDistance = 20f;
    [HideInInspector]
    public bool moveRight = true;

    [Header("Ground Enemy Attributes")]
    public float groundDetectorDistance = 2f;
    public Transform groundDetector;

    [Header("Flying Enemy Attributes")]
    public Transform spawnAttackParent;
    private GameObject cloneAttackPos;
    public GameObject spawnAttackPosObject;
    public float stoppingDistance = 7f;
    public float retreatDistance = 6f;
    private bool _spawnAttackIsCreated = false;
    private bool enemyBackingUp = true;
    private Vector2 originPlace;

    [Header("Shooting Flying Enemy Attributes")]
    public GameObject bulletPrefab;
    public Transform bulletsParent;
    private Bullets bullet;

    // >>>TIMER<<<
    private float _startingTimer = 0;
    public float _timer;

    // >>>ATTACK<<<
    private bool _swordAttack = false;
    private bool _bulletAttack = false;

    //>>>CHASING<<<

    // >>>CHANCES<<<
    public int chance;
    private bool _chanceIsOn = false;

    [Header("Reference")]
    public Transform player;
    private Vector2 playerDir;
    public GameObject wallDetector;
    private Player playerScrpt;
    [HideInInspector]
    public Vector2 lastPlayerPosition;

    // Testing
    [Header("Animation Testing")]
    public Animator anim;

    #endregion

    #region Start
    void Start()
    {
        _chanceIsOn = true;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
        if (flyingEnemy == true)
        {
            originPlace = new Vector2(transform.position.x, transform.position.y);
        }
        playerScrpt = player.GetComponent<Player>();
        _speed = initialSpeed;
        currentState = State.Patrol;
        _timer = _startingTimer;
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

            if (groundHit.collider == true)
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
            transform.position = Vector2.MoveTowards(transform.position, originPlace, _speed * Time.deltaTime);
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
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
                // Green Range
                //if (distanceToPlayer <= stoppingDistance && retreatDistance <= distanceToPlayer)
                //{ }

                // Yellow Range
               // if (distanceToPlayer <= retreatDistance)
                //{
                    // Enemy backing from player 
                    //transform.position = Vector2.MoveTowards(transform.position, player.position, -(_speed * 0.75f) * Time.deltaTime);
                //}

                // Just Outside Yellow Range
                if (distanceToPlayer <= retreatDistance + 2)
                { 
                    // If time more than or equal to 3 seconds
                    _timer += Time.deltaTime;

                    if (_chanceIsOn == true)
                    {
                        chance = Random.Range(0, 2);

                        #region when it is 0
                        if (chance == 0)
                        {
                            SwordAttack();
                            Debug.Log("Sword Attack");
                        }
                        #endregion

                        #region When it is 1
                        if (chance == 1)
                        {
                            SwordAttack();
                            //Shoot();
                        }
                        #endregion

                        _chanceIsOn = false;

                    }

                    // If it is in 6 seconds 
                    if (_timer >= 4)
                    {
                        // Reset timer and Chance
                        ResetTimerNStuff();
                    }


                }

                else
                {
                    currentState = State.Seek;
                    SeekPlayer();
                    ResetTimerNStuff();
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

        #region Enemy stops when close to player or not (Just The Ground Enemy)

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

        #endregion
    }
    #endregion

    void ResetTimerNStuff()
    {
        _timer = 0;
        chance = 0;
        _chanceIsOn = true;
        _spawnAttackIsCreated = false;
        Destroy(cloneAttackPos);
    }

    #region Damage To Player
    void DamagePlayer()
    {
        #region Ground Enemy
        if (groundEnemy == true)
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
            // It is in Bullet Script
        }
        #endregion
    }
    #endregion

    #region Shoot
    void Shoot()
    {
        GameObject projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletsParent);
        bullet = projectile.GetComponent<Bullets>();
    }
    #endregion

    #region Sword Attack
    void SwordAttack()
    {
        Vector3 spawnPos = player.transform.position * 1f;

        #region Spawn a Transform Position in front of enemy 
        if (_spawnAttackIsCreated == false)
        {
            cloneAttackPos = Instantiate(spawnAttackPosObject, spawnPos, Quaternion.identity, spawnAttackParent);
            _spawnAttackIsCreated = true;
        }
        #endregion

        transform.position = Vector2.MoveTowards(transform.position, spawnPos, (_speed * 2  ) * Time.deltaTime);
    }
    #endregion

    #region Trash

    #region Sword Attack      
    /*
    
    */

    #endregion

    #region Shoot

    #endregion

    #endregion

    #region Bunch of Art stuff
    // Don't how to draw a line so I'll use Sphere XD
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectorDistance);

        if (groundEnemy == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackDetector.position, attackRange);
        }
        if (flyingEnemy == true)
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
