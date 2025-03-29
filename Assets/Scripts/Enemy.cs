using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    public int health = 1;
    public float stunTime = 0f;

    public float attackRange = 10f;
    public float stopDistance = 1f;

    public float attackSpeed = 1.0f;

    private float attackTimer = 1.0f;
    public bool attacking = false;

    public float moveSpeed = 1f;

    public bool moving = true;

    private bool isDashing = false;
    private Vector2 dashDirection;

    public bool deadly = false;

    private float rotationSpeed = 10f;
    public float rotationOffset = 160f;

    public bool rotateTowardsPlayer = true;
    public Transform spriteTransform;
    public Rigidbody2D rb;

    public Player player;
    public Transform playerTransform;

    public Transform transform;

    public float distanceFromPlayer;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        rb = GetComponent<Rigidbody2D>();
        

    }

    // Update is called once per frame
    void Update()
    {

        distanceFromPlayer = (transform.position - playerTransform.position).magnitude;

        if(stunTime > 0f)
        {
            stunTime -= Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(0f,0f);
        }

        if(moveSpeed > 0 && player != null && moving && stunTime <= 0f)
        {
            Move();
        }
        AttackCheck();
    }

    public void Move()
        {
            if (distanceFromPlayer > stopDistance)
            {
                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            }
            
        if (rotateTowardsPlayer)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle + rotationOffset);

            spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation,targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public IEnumerator Dash(Vector2 targetPos, float time, float speed)
    {
        dashDirection = (targetPos - (Vector2)transform.position).normalized;
        dashDirection /= dashDirection.magnitude;
        isDashing = true;
        moving = false;
        float dashTime = time;

        while (dashTime > 0f)
        {
            moving = false;
            dashTime -= Time.deltaTime;

            if(stunTime > 0f)
            {
                isDashing = false;
                moving = true;
                yield break;
            }

            transform.Translate(dashDirection.normalized * speed * Time.deltaTime);

            if (dashTime < 0f)
            {
                moving = true;
                isDashing = false;

            }

            yield return null;
        }

    }

    public void TakeDamage(int damage)
    {
        stunTime = 1.0f;
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Award player moneys
        //Death particles
        Destroy(this.gameObject);
    }

    private void AttackCheck()
    {
        attackTimer -= Time.deltaTime;


        if (distanceFromPlayer < attackRange && stunTime <= 0f && attacking == false)
        {
            if(attackTimer <= 0f)
            {
                Debug.Log("Attack");
                UseAttack();
            }
        }
    }

    private void UseAttack()
    {
        StartCoroutine(Attack());
        attackTimer = attackSpeed;
    }

    public abstract IEnumerator Attack();

}
