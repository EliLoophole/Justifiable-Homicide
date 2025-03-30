using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{

    public float projectileSpeed = 3.0f;

    public int projectileCount = 1;
    public float timeBetweenProjectiles = 0.2f;

    public GameObject projectile;

    
    public override IEnumerator Attack()
    {
        int projectiles = projectileCount;
        float shotWait = 0f;

        while (projectiles > 0)
        {
            shotWait -= Time.deltaTime;

            if (shotWait <= 0f)
            {
                Vector3 direction = playerTransform.position - transform.position;
                float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

                Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90);

                GameObject newProjectile = Instantiate(projectile,transform.position,targetRotation);
                newProjectile.GetComponent<Projectile>().speed = projectileSpeed;

                projectiles--;
                shotWait = timeBetweenProjectiles;
            }
            yield return null;
        }
    }

}
