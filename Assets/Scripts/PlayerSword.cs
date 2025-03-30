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
    public float parryVelocity = 2.0f;
    public float KnockbackForce = 10f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if(enemy != null)
        {
            if (parrying && enemy.deadly && enemy.stunTime <= 0f)
            {
                HitEnemy(enemy);
            }
        }

        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (parrying)
            {
                HitProjectile(projectile);
            }
        }
    }

    public void ParryAnimation()
    {
        animator.enabled = true;
        if (transform.localScale.x < 0)
        {
            animator.Play("swordParry_Flipped", -1, 0f);
        }
        else
        {
            animator.Play("swordParry", -1, 0f);
        }
        StartCoroutine(DisableAnimatorAfterAnimation());
    }

    private IEnumerator DisableAnimatorAfterAnimation()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.enabled = false;
    }
    public void HitProjectile(Projectile projectile)
    {
        ParryAnimation();

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - projectile.transform.position).normalized;
        projectile.canHitEnemies = true;
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;

        projectile.transform.rotation = Quaternion.Euler(0,0,angle-90);
        projectile.speed *= parryVelocity;
    }
    public void HitEnemy(Enemy enemy)
    {
        ParryAnimation();
        Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        Vector2 Knockback = ((Vector2)(playerTransform.position - enemy.transform.position).normalized) * -KnockbackForce;
        enemyRB.AddForce(Knockback);

        enemy.Hurt(parryDamage);
        //bush did 9/11
    }
}
