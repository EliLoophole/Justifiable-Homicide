using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public bool parrying = false;
    private Rigidbody2D rb;
    private Transform transform;
    public Transform playerTransform;
    public Player player;

    public GameObject parryParticles;

    public int parryDamage = 1;
    public float parryVelocity = 2.0f;
    public float KnockbackForce = 10f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animator.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();

        if(enemy != null)
        {
            if (parrying && enemy.deadly && enemy.stunTime <= 0f)
            {
                HitEnemy(enemy,1,1f,parryParticles);
                OnSuccessfulParry();
            }
            else if (enemy.deadly && enemy.stunTime <= 0f)
            {
                HitEnemy(enemy,0,0.5f,null);
            }
        }

        Projectile projectile = other.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            if (parrying)
            {
                HitProjectile(projectile);
            }
            else if (projectile.blockable)
            {
                projectile.Kill();
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

        OnSuccessfulParry();

        if(projectile.destroyOnParry)
        {
            projectile.Kill();
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 direction = (mousePos - projectile.transform.position).normalized;
        projectile.canHitEnemies = true;
        float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;

        projectile.transform.rotation = Quaternion.Euler(0,0,angle-90);
        projectile.speed *= parryVelocity;
    }
    public void HitEnemy(Enemy enemy, int damage, float knockbackMultiplier,GameObject particles)
    {
        Rigidbody2D enemyRB = enemy.GetComponent<Rigidbody2D>();
        Vector2 Knockback = ((Vector2)(playerTransform.position - enemy.transform.position).normalized) * -KnockbackForce * knockbackMultiplier;
        enemyRB.velocity = Knockback;
        //glah
        enemy.Hurt(damage);
        //bush did 9/11
    }

    private void OnSuccessfulParry()
    {
        ParryAnimation();
        Instantiate(parryParticles,transform.position,Quaternion.identity);
        player.RefreshCooldowns(true,true);
    }

}
