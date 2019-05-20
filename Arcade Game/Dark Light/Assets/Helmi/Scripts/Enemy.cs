using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Base Enemy Attributes")]

    public State currentState;
    public float speed = 5f;

    [Header("Enemy Range Attributes")]

    public float distanceDetector = 5f;
    public Transform playerDetector;

    [Header("Patrol Attributes")]

    public float rayCastDistance = 2f;
    private bool moveRight = true;
    public Transform groundDetector;

    [Header("Chasing Player Attributes")]

    public Transform player;
    public Transform ChaseDot;

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

        PlayerInsideRange();
    }

    void Patrol()
    {
        BaseEnemyMovement();
    }

    void BaseEnemyMovement()
    {
        // Ground dectetor detects ground collider
        RaycastHit2D groundHit = Physics2D.Raycast(groundDetector.position, Vector2.down, rayCastDistance);

        // When there is ground collider, the ground detector wil do this 
        if (groundHit.collider == true)
        {
            // Moves enemy to right
            transform.Translate(Vector2.right * speed * Time.deltaTime);
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

    void SeekPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, ChaseDot.position, (speed * Time.deltaTime) / speed);
        BaseEnemyMovement();
    }

    void PlayerInsideRange()
    {
        RaycastHit2D playerHit = Physics2D.Raycast(playerDetector.position, Vector2.right, distanceDetector);

        if (playerHit.collider == true)
        {
            currentState = State.Seek;
        }
        else
        {
            currentState = State.Patrol;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    // Don't how to draw a line so I'll use Sphere XD
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceDetector);
    }
}
