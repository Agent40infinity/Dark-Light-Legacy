using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Attributes")]

    public State currentState;
    public float speed = 5f;

    [Header("Enemy Range Attributes")]

    public float range = 2f;
    private bool playerInsideRange = false;

    [Header("Patrol Attributes")]

    public float rayCastDistance = 2f;
    private bool moveRight = true;
    public Transform groundDetector;

    [Header("Chasing Player Attributes")]

    public Transform player;

    public enum State
    {
        Patrol,
        Seek
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        currentState = State.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Seek:
                SeekPlayer();
                break;
            default:
                Patrol();
                break;
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        BaseEnemyMovement();
    }

    void BaseEnemyMovement()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(groundDetector.position, Vector2.down, rayCastDistance);

        if (groundHit.collider == false)
        {
            if (moveRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                moveRight = false;
            }

            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                moveRight = true;
            }
        }
    }

    void SeekPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        BaseEnemyMovement();
    }

    void PlayerInsideRange()
    {
        // Setting this to infinity because the player is not within the enemy range 
        float playerShortestDistance = Mathf.Infinity;
        
        // Distance between enemy and player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // If the distance between the enemy and tower is less than the shortestdistance (distance to enemy will always be less than the shortest distance since it's infinity).
        if (distanceToPlayer < playerShortestDistance)
        {
            // Getting the shortest distance of the player (So within the range)
            playerShortestDistance = distanceToPlayer;
        }

        if(playerShortestDistance <= range)
        {
            Debug.Log("PLAYER IS INSIDE THE RANGE");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
