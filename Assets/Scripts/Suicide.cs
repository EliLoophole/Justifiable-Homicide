using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suicide : MonoBehaviour
{
    public float lifetime = 5f;

    private Projectile projectile;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Projectile>();

        StartCoroutine(destroyWithDelay());

    }

    private IEnumerator destroyWithDelay()
    {
        yield return new WaitForSeconds(lifetime);
        if (projectile != null)
        {
            projectile.Kill();
        }
        else
        {
            Destroy(this.gameObject, lifetime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
