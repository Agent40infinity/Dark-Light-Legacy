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

    #region Type of Enemy
    [Header("What type of enemy is this?")]
    public bool groundEnemy;
    public bool flyingEnemy;
    #endregion

    #region All Enemy Attack
    [Header("Attack Attributes")]
    public float attackDelay; // Delay for attack,,, Ground Enemy is set to 3f as default and Flying Enemy is set to 1f as default  
    #endregion

    #region Enemy Attributes
    [Header("Both Enemy Attributes")]
    public State currentState;
    public float initialSpeed = 5f; // Speed of enemy
    private float _speed; // Initial Speed will be stored to this variable
    #endregion

    #region Chasing Attributes
    [Header("Chasing Attributes")]
    public float playerDetectorDistance = 20f; // Distance where able to check where the player is
    [HideInInspector]
    public bool moveRight = true;
    #endregion

    #region Ground Enemy Attributes
    [Header("Ground Enemy Attributes")]
    public float groundDetectorDistance = 1f; // Raycast distance to the ground 
    public float attackDistance = 2f; // Attack Distance
    private float lastAttackTime; // Connecting with Attack Delay
    public Transform groundDetector; // Transform for Ground Detector
    public GameObject wallDetector; // Only for Ground Enemy
    #endregion

    #region Flying Enemy Attributes
    [Header("Flying Enemy Attributes")]
    public float attackSpeed = 5f; // Speed of the attack when it moves fast to player.
    public float bulletSpeed = 20f; // Speed of the bullet.
    public float stoppingDistance = 8f; // Stopping Distance.
    public float retreatDistance = 6f; // Retreat Distance.
    public float flyAttackDistance = 1f; // Fly Attack Distance.
    public GameObject bulletPrefab; // Bullet prefab
    public Transform bulletsParent; // This is just for making it tidy (bullets will spawn under this GameObject in inspector)
    private Bullets bullet; // Bullet
    private Vector2 originPlace; // Set where the enemy needs to go after chasing
    #endregion

    // >>>TIMER<<<
    private float _timer = 0; // Timer for Attack and chance
    private float attackTimer = 0f; // AttackTimer
    private float revertTimer = 0f; // RevertTimer

    // >>>CHANCES<<<
    private int chance;
    private bool _chanceIsOn = false;

    #region Reference
    [Header("Reference")]
    public Transform player;
    public float distanceToPlayer; // Distance between enemy and player
    private Player playerScrpt; // Plyer Script
    private Vector2 lastEnemyPosition; // Store Last Enemy Position
    private Vector2 attackPos; // Store Attack Pos
    #endregion

    #region Testing
    // Testing
    [Header("Animation Testing")]
    public Animator anim;
    #endregion

    #endregion

    #region Start
    void Start()
    {
        _chanceIsOn = true;
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerScrpt = player.GetComponent<Player>();
        _speed = initialSpeed;
        currentState = State.Patrol;

        if (flyingEnemy == true)
        {
            originPlace = new Vector2(transform.position.x, transform.position.y);
        }
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
            case State.Attack:

                attackTimer += attackSpeed * Time.deltaTime;

                if (distanceToPlayer <= flyAttackDistance)
                {
                    playerScrpt.beenHit = true;
                }

                // If the attack timer is not at the end?
                if (attackTimer <= 1f)
                {
                    // Lerp Enemy to Attack Point
                    transform.position = Vector3.Lerp(lastEnemyPosition, attackPos, attackTimer);
                }

                else
                {
                    // Increase the revertTimer
                    revertTimer += attackSpeed * Time.deltaTime;

                    // If it's less than zero
                    if (revertTimer <= 1f)
                    {
                        // Lerp Enemy to Last Point
                        transform.position = Vector3.Lerp(attackPos, lastEnemyPosition, revertTimer);
                    }
                }

                break;
        }

        // the distance between enemy and player
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
    public void BaseEnemyMovement()
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
    void SeekPlayer(float speed)
    {
        #region Ground Enemy
        if (groundEnemy == true)
        {
            Vector2 seekPosition = player.transform.position;
            seekPosition.y = transform.position.y;
            transform.position = Vector2.MoveTowards(transform.position, seekPosition, speed * Time.deltaTime);
        }
        #endregion

        #region Flying Enemy
        if (flyingEnemy == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        #region Chase player or not
        // Player inside the red Circle Range
        if (distanceToPlayer <= playerDetectorDistance)
        {
            #region Ground Enemy
            if (groundEnemy == true)
            {
                currentState = State.Seek;
                SeekPlayer(_speed);
            }
            #endregion

            #region Flying Enemy
            if (flyingEnemy == true)
            {
                // Yellow Range
                if (distanceToPlayer <= retreatDistance)
                {
                    // Enemy backing from player 
                    transform.position = Vector2.MoveTowards(transform.position, player.position, -(_speed * 0.75f) * Time.deltaTime);
                }
                // Just Outside Yellow Range
                if (distanceToPlayer <= stoppingDistance)
                {

                    // If time more than or equal to 3 seconds
                    _timer += Time.deltaTime;

                    if (_chanceIsOn == true)
                    {
                        chance = Random.Range(0, 2);
                        #region when it is 0
                        if (chance == 0)
                        {
                            StartCoroutine(SwordAttack());
                        }
                        #endregion
                        #region When it is 1
                        if (chance == 1)
                        {
                            Shoot();
                        }
                        #endregion
                        _chanceIsOn = false;
                    }
                    // If it is in 4 seconds 
                    if (_timer >= 4)
                    {
                        // Reset timer and Chance
                        ResetTimerNStuff();
                    }
                }
                else
                {
                    currentState = State.Seek;
                    SeekPlayer(_speed);
                    ResetTimerNStuff();
                }
            }
            #endregion
        }

        // Player outside the red Circle Range
        else
        {
            // If it is not attacking then Patrol
            if(currentState != State.Attack)
            {
                currentState = State.Patrol;
            }

            BaseEnemyMovement();
        }
        #endregion

        #region Enemy stops when close to player or not (Just The Ground Enemy)

        #region Ground Enemy
        if (groundEnemy == true)
        {
            // If player inside the hit box range
            if (distanceToPlayer <= attackDistance)
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

    #region Reset
    void ResetTimerNStuff()
    {
        _timer = 0;
        chance = 0;
        _chanceIsOn = true;
    }
    #endregion

    #region Damage To Player
    void DamagePlayer()
    {
        #region Ground Enemy
        if (groundEnemy == true)
        {
            //==============
            // Referenced from this website: https://www.youtube.com/watch?v=LqCJowEQFBc
            if (Time.time >= lastAttackTime + attackDelay)
            {
                playerScrpt.beenHit = true;
                anim.SetBool("IsAttack", true);
                lastAttackTime = Time.time;
            }
            //============== 
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
    IEnumerator SwordAttack()
    {
        // Reset timers
        revertTimer = 0f;
        attackTimer = 0f;
        
        attackPos = player.position; // Store the last player position
        lastEnemyPosition = transform.position; // Store the Last Enemy Position

        var prevState = currentState; // Store Previous State (State.Seek)

        currentState = State.Attack; // Switch to Attack State

        yield return new WaitForSeconds(attackDelay); 

        currentState = prevState; // Switch to Seek State
    }
    #endregion


    #region Gizmos
    // Don't how to draw a line so I'll use Sphere XD
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectorDistance);

        if (groundEnemy == true)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
        if (flyingEnemy == true)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, retreatDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, flyAttackDistance);

        }
    }
    #endregion

    public enum State
    {
        Patrol,
        Seek,
        hit,
        Attack
    }
}
