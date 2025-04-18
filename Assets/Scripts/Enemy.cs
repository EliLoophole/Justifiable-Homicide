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

    public GameObject deathParticles;

    public Animator animator;
    private WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        rb = GetComponent<Rigidbody2D>();
        
        waveManager = FindObjectOfType<WaveManager>();
        
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(UpdateDistance());
    }

    // Update is called once per frame
    void Update()
    {

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

    private IEnumerator UpdateDistance()
    {
        if(playerTransform != null)
        {
            distanceFromPlayer = (transform.position - playerTransform.position).magnitude;
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateDistance());
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

        Vector2 direction = targetPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

        spriteTransform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        while (isDashing)
        {
            moving = false;
            dashTime -= Time.deltaTime;

            if(stunTime <= 0f)
            {
                rb.velocity = (dashDirection * speed);
            }
            else
            {
                isDashing = false;
                moving = true;
                yield break;
            }

            if (dashTime < 0f)
            {
                moving = true;
                isDashing = false;

            }

            yield return null;
        }

    }

    public void Hurt(int damage)
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
        Instantiate(deathParticles,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
        waveManager.TestWin();
    }

    private void AttackCheck()
    {
        attackTimer -= Time.deltaTime;


        if (distanceFromPlayer < attackRange && stunTime <= 0f && attacking == false)
        {
            if(attackTimer <= 0f)
            {
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

    void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject other = collision.gameObject;

            Player player = other.GetComponent<Player>();
            if (player != null && deadly)
            {
                player.Hurt();
            }
            
        }


}
