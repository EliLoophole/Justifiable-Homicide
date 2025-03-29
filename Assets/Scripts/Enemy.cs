using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 1;

    public float moveSpeed = 1f;
    public float dashSpeed = 0f;

    public bool moving = true;

    public bool DashButton = false;

    private bool isDashing = false;
    private Vector2 dashDirection;

    public bool deadly = false;

    private float rotationSpeed = 10f;
    public float rotationOffset = 160f;

    public bool rotateTowardsPlayer = true;
    private Transform spriteTransform;

    private Player player;
    private Transform playerTransform;

    public Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();

        player = FindObjectOfType<Player>();
        playerTransform = player.GetComponent<Transform>();
        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        

    }

    // Update is called once per frame
    void Update()
    {

        if(DashButton)
        {
            StartCoroutine(Dash(playerTransform.position, 0.1f, dashSpeed));
            DashButton = false;
        }

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

            spriteTransform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator Dash(Vector2 targetPos, float time, float speed)
    {
        Debug.Log("dashing it");

        dashDirection = (targetPos - (Vector2)transform.position).normalized;
        dashDirection /= dashDirection.magnitude;
        isDashing = true;
        moving = false;
        float dashTime = time;

        while (dashTime > 0f)
        {
            moving = false;
            dashTime -= Time.deltaTime;
            transform.Translate(dashDirection.normalized * dashSpeed * Time.deltaTime);

            if (dashTime < 0f)
            {
                moving = true;
                isDashing = false;

            }

            yield return null;
        }


    }

}
