using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    //General:
    [Header("General")]
    public string[] enemyTypes;
    public EnemyType enemyType;
    public float moveSpeed = 4f;
    public float activationDistance = 12f;
    public float chaseDistance = 20f;
    public bool canChase = true;
    public bool beenActivated = false;
    public bool facing = true; //true = left, false = right.

    //Damage Management:
    [Header("Damage Management")]
    public float damage;
    public float attackTime = 3f;
    public float attackTimer;

    //Health Management:
    [Header("Health Management")]
    public int curHealth;
    public int maxHealth;
    public bool dead = false;

    //SentryOrb:
    [Header("Sentry Orb")]
    public float playerDistance;
    public float retreatDistance = 6f;
    public float stoppingDistance = 8f;

    //SentryKnight:
    [Header("Sentry Knight")]

    //References:
    [Header("References")]
    public Player player;
    public Animator anim;
    public GameObject enemy;
    public GameObject bulletPrefab;
    #endregion

    #region Public Properties
    public EnemyType GetNameToEnum()
    {
        enemyTypes = System.Enum.GetNames(typeof(EnemyType)); //Creates an array to store the name of each PerkType
        for (int i = 0; i < enemyTypes.Length; i++) //Checks all perks.
        {
            if (enemy.name.Contains(enemyTypes[i])) //Checks whether or not the attached gameObjects contains the name of the perk and allows the perk to be purchased.
            {
                return enemyType = (EnemyType)System.Enum.Parse(typeof(EnemyType), enemyTypes[i]);
            }
        }
        return EnemyType.end;
    }
    #endregion

    #region General
    public void Start()
    {
        enemy = gameObject;
        anim = gameObject.GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        bulletPrefab = Resources.Load("Prefabs/Enemies/Projectile") as GameObject;
        attackTimer = attackTime;
        GetNameToEnum();

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

    public void Update()
    {
        playerDistance = Vector2.Distance(enemy.transform.position, player.transform.position);

        Movement();
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
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(enemy.transform.position.x, enemy.transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y), activationDistance);

                if (hit.collider.tag == "Player")
                {
                    //anim.SetTrigger("isActivated");
                    beenActivated = true;
                }

                if (playerDistance <= activationDistance)
                {
                    beenActivated = true;
                }

                if (beenActivated)
                {
                    if (playerDistance <= chaseDistance)
                    {
                        if (canChase)
                        {
                            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                        }

                        if (playerDistance <= stoppingDistance)
                        {
                            canChase = false;

                            if (attackTimer <= 0)
                            {
                                Attack();
                                attackTimer = attackTime;
                            }
                            else
                            {
                                attackTimer -= Time.deltaTime;
                            }

                            if (playerDistance <= retreatDistance)
                            {
                                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, -moveSpeed * Time.deltaTime);
                            }
                        }
                        else
                        {
                            canChase = true;
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region Damage
    public void Attack()
    {
        GameObject bullet = Instantiate(bulletPrefab, enemy.transform.position, Quaternion.identity);
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
    SentryKnight,
    end
}