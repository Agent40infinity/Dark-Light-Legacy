using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------/
 * Script created by Aiden Nathan.
 *---------------------------------*/

public class EnemyHealth : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;
    public bool Dead = false;
    public Animator anim;
    public GameObject enemy;

    public void Start()
    {
        enemy = gameObject;
        anim = gameObject.GetComponent<Animator>();
        switch (enemy.name)
        {
            case "GroundEnemy":
                maxHealth = 10;
                break;
            case "FlyingEnemy":
                maxHealth = 6;
                break;
        }
        curHealth = maxHealth;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("Death");
        }
    }

    public void TakeDamage(int damage)
    {
        if (curHealth != 0)
        {
            curHealth -= damage;
        }
        else
        {
            StartCoroutine("Death");
        }
    }

    public IEnumerator Death()
    {
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        Destroy(enemy);
    }
}
