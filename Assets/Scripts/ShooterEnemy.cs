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
            if (shotWait <= 0f)
            {
                Instantiate(projectile,transform.position,Quaternion.identity);
                projectiles--;
                Debug.Log("Shoot");
            }
            yield return null;
        }
    }

}
