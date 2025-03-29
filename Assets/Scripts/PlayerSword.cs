using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public bool parrying = false;
    private Rigidbody2D rb;
    private Transform transform;
    public Transform playerTransform;

    public int parryDamage = 1;
    public float KnockbackForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("collide");
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if(enemy != null)
        {
            if (parrying && enemy.deadly && enemy.stunTime <= 0f)
            {
                Debug.Log("Parry");
                HitEnemy(enemy);
            }
        }
    }

    public void HitEnemy(Enemy enemy)
    {
        Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        Vector2 Knockback = ((Vector2)(playerTransform.position - enemy.transform.position).normalized) * -KnockbackForce;
        enemyRB.AddForce(Knockback);

        Debug.Log(Knockback);

        enemy.TakeDamage(parryDamage);

    }
}
