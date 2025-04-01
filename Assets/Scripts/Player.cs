using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int lives = 3;

    [SerializeField]
    private float movementSpd = 1f;
    [SerializeField]
    private float dashSpd = 5f;

    private float dashTimer;
    public float dashCooldown = 3f;

    private float parryTimer = 0f;
    public float parryCooldown= 2f;

    public Color parryColor;

    public bool canMove = true;

    public float parryDuration = 0.2f;

    public GameObject deathParticles;

    private GameManager gameManager;

    private Rigidbody2D rb;
    private Vector2 movementDir;
    [SerializeField]
    private GameObject sword;
    private PlayerSword swordScript;
    private Animator swordAnimator;
    //private Transform transform;

    private SpriteRenderer swordSprite;
    private Color originalColor;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite frontView;
    [SerializeField]
    private Sprite backView;

    [SerializeField]
    private Animator playerAnimator;

    void Start()
    {
        swordSprite = sword.GetComponentInChildren<SpriteRenderer>();
        swordScript = sword.GetComponent<PlayerSword>();
        swordAnimator = sword.GetComponent<Animator>();

        gameManager = FindObjectOfType<GameManager>();

        //transform = this.transform;

        rb = GetComponent<Rigidbody2D>();

        originalColor = swordSprite.color;
    }

    // Update is called once per frame
    void Update()
    {

        Move();

        if(Input.GetMouseButtonDown(1) && parryTimer <= 0f)
        {
            StartCoroutine(Parry());
            Debug.Log("Parrying");
        }
        else if(!swordScript.parrying)
        {
            parryTimer -= Time.deltaTime;

            float shade = 0.6f - 0.3f*(parryTimer / parryCooldown);

            if(parryTimer/parryCooldown < 0.001) shade = 1f;

            Color rechargingColor = new Color(shade,shade,shade);

            swordSprite.color = rechargingColor;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
    

        if (mousePos.y > transform.position.y+2)
        {
            //Debug.Log("Mouse y: " + mousePos.y);
            spriteRenderer.sprite = backView;
        }
        else
        {
            //Debug.Log("Mouse y: "+ mousePos.y);
            spriteRenderer.sprite = frontView;
        }


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
        parryTimer = parryCooldown;
        swordSprite.color = parryColor;
        canMove = false;
        swordScript.parrying = true;

        yield return new WaitForSeconds(parryDuration);

        swordScript.parrying = false;
        swordSprite.color = originalColor;
        canMove = true;
    }

    public void RefreshCooldowns(bool dash, bool parry)
    {
        if(dash) dashTimer = 0f;
        if(parry) parryTimer = 0f;
    }

    void FlipObject()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;



        Vector3 swordScale = sword.transform.localScale;
        swordScale.y = -swordScale.y;
        swordScale.x = -swordScale.x;
        sword.transform.localScale = swordScale;
    }

    private void Move()
    {
        if (canMove)
        {

            movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            if(Input.GetKeyDown(KeyCode.Space) && dashTimer <= 0f)
            {
                StartCoroutine(Dash(movementDir));
            }
            else
            {
                dashTimer -= Time.deltaTime;
            }
    
            transform.Translate(movementDir * movementSpd * Time.deltaTime);
            if (movementDir.magnitude > 0.1f)
            {
                playerAnimator.SetBool("walking",true);
            }
            else
            {
                playerAnimator.SetBool("walking",false);
            }
        }
        else
        {
            playerAnimator.SetBool("walking",false);
        }

    }

    public IEnumerator Dash(Vector2 direction)
    {
        Debug.Log("Dashing");

        canMove = false;

        rb.velocity += direction * dashSpd;
        dashTimer = dashCooldown;

        yield return new WaitForSeconds(0.4f);

        canMove = true;
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
        gameManager.Lose();
        Destroy(this.gameObject);
        Instantiate(deathParticles,transform.position,Quaternion.identity);
    }
}
