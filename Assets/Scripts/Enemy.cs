using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 1;
    public float moveSpeed = 1f;

    public bool moving = true;

    public bool dashTime = 0f;

    public bool deadly = false;

    private float rotationSpeed = 1f;

    public bool rotateTowardsPlayer = true;

    private Player player;
    private Transform playerTransform;

    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>()
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MoveSpeed > 0 && player != null && moving)
        {
            Move();
        }

    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        if (rotateTowardsPlayer)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime * rotationSpeed) LookAt(player);
        }
    }

    public void Dash(Vector2 targetPos, float time, float speed)
    {
        dashing
    }

    private void HandleDash()
    {
        if (dashTime > 0f)
        {
            moving = false;
            dashTime -= Time.deltaTime;
        }
    }

}
