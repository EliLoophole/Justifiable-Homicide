using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private float movementSpd = 1f;
    public bool canMove = true;

    public float parryDuration = 0.2f;

    private Rigidbody2D rb;
    private Vector2 movementDir;
    [SerializeField]
    private GameObject sword;
    private PlayerSword swordScript;

    private SpriteRenderer swordSprite;

    void Start()
    {
        swordSprite = sword.GetComponentInChildren<SpriteRenderer>();
        swordScript = sword.GetComponent<PlayerSword>();

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Parry());
        }

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

    public IEnumerator Parry()
    {
        Color originalColor = swordSprite.color;
        swordSprite.color = Color.red;
        canMove = false;
        swordScript.parrying = true;

        yield return new WaitForSeconds(parryDuration);

        swordScript.parrying = false;
        swordSprite.color = originalColor;
        canMove = true;
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

    private void Move()
    {
        movementDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = movementDir * movementSpd;

    }

    private void FixedUpdate()
    {
        if(canMove) Move();
    }

    public void Hurt()
    {
        lives--;
        if(lives < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        print("man im dead");
    }
}
