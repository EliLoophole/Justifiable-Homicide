using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : Enemy
{
    public float dashLength = 0.1f;
    public float dashSpeed = 50f;
    
    public override IEnumerator Attack()
    {
        deadly = true;
        yield return StartCoroutine(Dash(playerTransform.position, dashLength, dashSpeed));
        deadly = false;
    }
}
