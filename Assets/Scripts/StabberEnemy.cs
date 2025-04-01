using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : Enemy
{
    public float dashLength = 0.1f;
    public float windupTime = 0.5f;
    public float dashSpeed = 50f;
    
    public override IEnumerator Attack()
    {
        animator.SetTrigger("Windup");

        moving = false;
        yield return new WaitForSeconds(windupTime);

        deadly = true;
        yield return StartCoroutine(Dash(playerTransform.position, dashLength, dashSpeed));
        deadly = false;
    }
}
