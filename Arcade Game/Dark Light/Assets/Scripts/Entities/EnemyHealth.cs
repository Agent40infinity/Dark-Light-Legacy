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
    public GameObject Enemy;

    public void Start()
    {
        switch (Enemy.name)
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

    public void TakeDamage(int damage)
    {
        if (curHealth != 0)
        {
            curHealth -= damage;
        }
        else
        {
            Dead = true;
        }
    }
}
