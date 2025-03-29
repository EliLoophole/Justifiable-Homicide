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

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        
        float angle = Mathf.Atan2(mousePos.y - transform.position.y,mousePos.x - transform.position.x);

        Vector3 newPositon = transform.position + new Vector3(Mathf.Cos(angle),Mathf.Sin(angle)*.5f);
        sword.transform.position = newPositon;

        float rotationAngle = angle*Mathf.Rad2Deg;
        sword.transform.rotation = Quaternion.Euler(0,0,rotationAngle);

        if (mousePos.x >transform.position.x && transform.localScale.x<0)
        {
            FlipObject();
        }
        else if (mousePos.x < transform.position.x && transform.localScale.x > 0)
        {
            FlipObject();
        }
    }
    void FlipObject()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        

        Vector3 swordScale = sword.transform.localScale;
        print(swordScale);
        swordScale.y = -swordScale.y;
        swordScale.x = -swordScale.x;
        print(swordScale);
        sword.transform.localScale = swordScale;
        print("Sword Flipped!");
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
