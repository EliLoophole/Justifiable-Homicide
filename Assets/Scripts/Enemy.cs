using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 1;

    public float moveSpeed = 1f;
    public float dashSpeed = 0f;

    public bool moving = true;

    private float dashTime = 0f;
    private bool isDashing = false;
    private Vector2 dashDirection;

    public bool deadly = false;

    private float rotationSpeed = 10f;
    public float rotationOffset = 160f;

    public bool rotateTowardsPlayer = true;

    private Player player;
    private Transform playerTransform;

    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();

        Dash(playerTransform.position, 1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {

        if(moveSpeed > 0 && player != null && moving)
        {
            Move();
        }

    }

    public void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        if (rotateTowardsPlayer)
        {
            Vector3 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angle + rotationOffset);

            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator Dash(Vector2 targetPos, float time, float speed)
    {
        Vector2 pos = new Vector2 (transform.position.x,transform.position.y);

        dashDirection = targetPos - pos;
        isDashing = true;
        moving = false;
        dashTime = time;
        
        while (dashTime > 0f)
        {
            moving = false;
            dashTime -= Time.deltaTime;
            transform.Translate(dashDirection.normalized * dashSpeed * Time.deltaTime);

            if (dashTime < 0f)
            {
                moving = true;
                isDashing = false;
                yield return null;
            }

        }


    }

}
