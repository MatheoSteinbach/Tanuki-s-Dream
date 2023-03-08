using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKappaAI : FSM
{
    public enum FSMStates
    {
        Patrol, Chase, Attack, Dead
    }
    [SerializeField] Sprite deadSprite;
    [SerializeField] GameObject projectile;

    [SerializeField] FSMStates currentState = FSMStates.Patrol;
    [SerializeField] int health = 10;
    [SerializeField] private float minimumDistance = 2f;

    [SerializeField] float speed = 5f;
    public int CurrentHealth { get; set; }

    private float attackRate = 1.5f;
    private float elapsedTime;
    private float jumpTime;
    private bool isDead;

    private SpriteRenderer sprite;
    [SerializeField]
    [Range(0f, 1f)]
    float lerpTime;
    [SerializeField] Color32 color;
    protected override void Initialize()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CurrentHealth = health;
        FindNextDestination();
    }

    protected override void FSMUpdate()
    {
        switch (currentState)
        {
            case FSMStates.Patrol:
                StatePatrol();
                break;
            case FSMStates.Chase:
                StateChase();
                break;
            case FSMStates.Attack:
                StateAttack();
                break;
            case FSMStates.Dead:
                StateDead();
                break;
            default:
                break;
        }


        jumpTime += Time.deltaTime;
        if (CurrentHealth <= 0)
        {
            currentState = FSMStates.Dead;
        }
    }

    private void StatePatrol()
    {
        if (Vector2.Distance(transform.position, destinationPos) <= 1f)
        {
            FindNextDestination();
        }
        else if (Vector2.Distance(transform.position, playerTransform.position) <= 7f)
        {
            currentState = FSMStates.Chase;
        }

        MoveTowardsDestination();
    }
    private void StateChase()
    {
        elapsedTime = 0;
        sprite.color = Color.Lerp(sprite.color, Color.white, lerpTime);
        destinationPos = playerTransform.position;

        float distanceToAttack = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToAttack <= 6f)
        {
            currentState = FSMStates.Attack;
        }
        if (distanceToAttack >= 8f)
        {
            currentState = FSMStates.Patrol;
        }

        if (Vector2.Distance(transform.position, playerTransform.position) > minimumDistance)
        {
            MoveTowardsDestination();
        }
    }
    private void StateAttack()
    {
        var originalColor = sprite.color;
        elapsedTime += Time.deltaTime;
        if (elapsedTime < 3 && elapsedTime > 1)
        {
            sprite.color = Color.Lerp(sprite.color, color, lerpTime);
        }
        else
        {
            sprite.color = Color.Lerp(sprite.color, Color.white, lerpTime);
        }
        destinationPos = playerTransform.position;

        float distanceToAttack = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToAttack < 6f)
        {
            if (Vector2.Distance(transform.position, playerTransform.position) > minimumDistance)
            {
                MoveTowardsDestination();
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, -speed * Time.deltaTime);
            }

            currentState = FSMStates.Attack;
        }
        else if (distanceToAttack >= 8f)
        {
            currentState = FSMStates.Patrol;
        }

        // Attack here
        Attack();
    }
    private void StateDead()
    {
        if (!isDead)
        {
            isDead = true;
            sprite.sprite = deadSprite;
            GetComponent<BoxCollider2D>().isTrigger = true;
            transform.position = transform.position;
            // destroy?
        }
    }


    private void Attack()
    {
        if (elapsedTime >= attackRate)
        {
            Instantiate(projectile, transform.position, transform.rotation);
            elapsedTime = 0;
        }
    }
    private void FindNextDestination()
    {
        int randomIndex = Random.Range(0, wanderPoints.Length);
        destinationPos = wanderPoints[randomIndex].transform.position;
    }

    private void MoveTowardsDestination()
    {
        transform.position = Vector2.MoveTowards(transform.position, destinationPos, speed * Time.deltaTime);
    }

    public void SetDeadState()
    {
        currentState = FSMStates.Dead;
    }
}
