using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D body;
    public Animator pAnimator;
    [SerializeField]
    Image damageIndicator;
    [SerializeField]
    LivesUI livesUI;

    public float horizontal;
    public float vertical;
    public bool speed;
    bool interact;
    float moveLimiter = 0.7f;

    public float moveSpeed = 20.0f;
    private bool slowed;
    private float aSpeed;

    [SerializeField]
    private int lives;
    private bool invincible;

    public bool key = false;
    public bool atticKey = false;
    public bool dog = false;

    public GameObject lastHit;
    public Vector2 collision = Vector2.zero;

    public static Player Instance { get; set; }

    void Start()
    {
        //Ensures there is only one game controller in the game at a time.
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        body = GetComponent<Rigidbody2D>();
        pAnimator = GetComponent<Animator>();
    }



    void Update()
    {
        if (!GameController.Instance.paused && !GameController.Instance.menuActive)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            interact = Input.GetKeyDown(KeyCode.Space);
        }

        else
        {
            horizontal = 0f;
            vertical = 0f;
            aSpeed = 0f;

            interact = false;
        }

        pAnimator.SetFloat("Horizontal_input", horizontal);
        pAnimator.SetFloat("Vertical_input", vertical);
        aSpeed = new Vector2(horizontal, vertical).sqrMagnitude;
        pAnimator.SetFloat("Speed", aSpeed);

        if (aSpeed != 0)
        {
            pAnimator.SetFloat("Last Horizontal", horizontal);
            pAnimator.SetFloat("Last Vertical", vertical);
        }

        body.constraints = RigidbodyConstraints2D.FreezeRotation;

        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;

            if (lastHit == gameObject.CompareTag("StareEnemy"))
            {
                //destroy enemy
            }

            Debug.Log("look");
        }
    }

    void FixedUpdate()
    {
        //Slows movement on the diagonal to improve the feel
        if (horizontal != 0 && vertical != 0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        //If moving through a slow tile, speed is halved
        if (slowed)
        {
            horizontal /= 2;
            vertical /= 2;
        }


        body.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
        //Checking if it has hit the player
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("StareEnemy"))
        {
            if (enemyRB != null)
            {
                enemyRB.isKinematic = true;
            }
            TakeDamage(1);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Rigidbody2D enemyRB = collision.gameObject.GetComponent<Rigidbody2D>();
        //Checking if it has hit the player
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("StareEnemy"))
        {
            if (enemyRB != null)
            {
                enemyRB.isKinematic = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!invincible)
        {
            StartCoroutine(IFrames());
            lives -= damage;
            livesUI.ReduceLife(lives);

            StartCoroutine(FadeInOut());

            //Add SFX

            if(lives <= 0)
            {
                GameController.Instance.Died();
            }
        }

    }

    IEnumerator FadeInOut()
    {
        damageIndicator.gameObject.SetActive(true);
        yield return new WaitForSeconds(0f);
        damageIndicator.CrossFadeAlpha(1, 2, false);
        yield return new WaitForSeconds(0f);
        damageIndicator.CrossFadeAlpha(0, 2, false);
        yield return new WaitForSeconds(0f);
        damageIndicator.gameObject.SetActive(false);
    }

    IEnumerator IFrames()
    {
        invincible = true;

        yield return new WaitForSeconds(1.5f);

        invincible = false;
    }

    //Teleports the player to the correct door when changing floors
    public void TeleportTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the trigger is a slow tile
        if (collision.gameObject.CompareTag("Slow"))
        {
            slowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if leaving a slow tile
        if (collision.gameObject.CompareTag("Slow"))
        {
            slowed = false;
        }
    }

    
}
