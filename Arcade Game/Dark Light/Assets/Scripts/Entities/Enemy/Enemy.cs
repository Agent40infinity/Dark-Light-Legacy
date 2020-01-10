using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    //General:
    [Header("General")]
    public EnemyType enemyType;
    public float moveSpeed;
    public float damage;
    public float rotationTime;
    public bool facing = true; //true = left, false = right.

    //Health Management:
    [Header("Health Management")]
    public int curHealth;
    public int maxHealth;
    public bool dead = false;

    //SentryOrb:
    [Header("Sentry Orb")]
    public float retreatDistance = 6f;
    public float stoppingDistance = 8f;

    //SentryKnight:
    [Header("Sentry Knight")]

    //References:
    [Header("References")]
    public Player player;
    public Animator anim;
    public GameObject enemy;
    #endregion

    #region General
    public void Start()
    {
        enemy = gameObject;
        anim = gameObject.GetComponent<Animator>();

        switch (enemyType)
        {
            case EnemyType.SentryKnight:
                maxHealth = 10;
                break;
            case EnemyType.SentryOrb:
                maxHealth = 6;
                break;
        }
        curHealth = maxHealth;
    }
    #endregion

    #region Movement
    public void Movement()
    {
        switch (enemyType)
        {
            case EnemyType.SentryKnight:

                break;
            case EnemyType.SentryOrb:

                break;
        }
    }
    #endregion

    #region Health Management
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
        yield return new WaitForSeconds(4);
        Destroy(enemy);
    }
    #endregion 
}

public enum EnemyType
{
    SentryOrb,
    SentryKnight
}