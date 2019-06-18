using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    public float speed = 40f;

    private Transform player;
    private Player playerScrpt;
    public Vector2 targetShoot;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerScrpt = player.GetComponent<Player>();
        targetShoot = new Vector2(player.position.x, player.position.y);
    }

    void Update()
    {
        if (player)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetShoot, speed * Time.deltaTime);

            if(transform.position.x == targetShoot.x && transform.position.y == targetShoot.y)
            {
                Destroy(gameObject);
            }

            if (transform.position.x == player.transform.position.x && transform.position.y == player.transform.position.y)
            {
                playerScrpt.beenHit = true;
            }
        }
    }

}
