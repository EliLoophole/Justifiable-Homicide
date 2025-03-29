using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float movementSpd = 1f;
    private Rigidbody2D rb;
    private Vector2 movementDir;
    [SerializeField]
    private GameObject sword;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        print(angle);

        Vector3 newPositon = transform.position + new Vector3(Mathf.Cos(angle),Mathf.Sin(angle));
        sword.transform.position = newPositon;

        //float rotationAngle = angle*Mathf.Rad2Deg;
        //sword.transform.rotation = Quaternion.Euler(0,0,rotationAngle);
    }

    private void FixedUpdate()
    {
        rb.velocity = movementDir * movementSpd;
    }

    private void Die()
    {
        print("man im dead");
    }
}
