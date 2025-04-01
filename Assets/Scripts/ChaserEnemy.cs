using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerEnemy : Enemy
{
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
    }
}
