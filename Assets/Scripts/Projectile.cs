using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 1.0f;
    public bool followPlayer = false;
    private Transform transform;
    private Transform playerTransform;

    public bool gracePeriod = false;

    public bool canHitEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        playerTransform = FindObjectOfType<Player>().transform;

        if (gracePeriod && canHitEnemies) StartCoroutine(GracePeriod());
    }

    // Update is called once per frame
    void Update()
    { 
        if(followPlayer == false)
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        }
        else
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            direction /= direction.magnitude;

            transform.Translate(direction * Time.deltaTime * speed, Space.World);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;

        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.Hurt();
            Destroy(this.gameObject);
        }
        else
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && canHitEnemies)
            {
                enemy.Hurt(1);
                Destroy(this.gameObject);
            }
        
        }

    }

    private IEnumerator GracePeriod()
    {
        canHitEnemies = false;
        yield return new WaitForSeconds(1f);
        canHitEnemies = true;
    }
}
