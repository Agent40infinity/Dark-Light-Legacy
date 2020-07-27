using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public Player player;
    public GameObject enemy;

    public void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    public void Update()
    {
        Movement();
    }

    public void Movement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerMovement.enemyPos = gameObject.transform;
            player.beenHit = true;
            Destroy(this.gameObject);
        }
        if (other.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
