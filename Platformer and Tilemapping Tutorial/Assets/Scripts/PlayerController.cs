using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Animator anim;
    private Rigidbody2D rb2d;
    public float speed;
    public float jumpForce;
    public Text countText;
    public Text winText;
    public Text livesText;
    public Text loseText;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private int count;
    private int lives;
    private bool facingRight;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    private LayerMask whatIsGround;
    private bool isGrounded;
    private bool jump;
    private Animator myAnimator;

    void Start()
    {
        anim = GetComponent<Animator>();
        facingRight = true;
        rb2d = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        lives = 3;
        winText.text = "";
        loseText.text = "";

        SetAllText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            musicSource.Stop();

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            musicSource.Stop();

        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            musicSource.loop = true;
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            musicSource.loop = false;
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
    private void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");

        isGrounded = IsGrounded();

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        myAnimator.SetFloat("speed", Mathf.Abs(moveHorizontal));

        Flip(moveHorizontal);

        HandleLayers();

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                myAnimator.SetTrigger("jump");
            }
            else
            {
                myAnimator.ResetTrigger("jump");
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetAllText();

        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            SetAllText();
        }
        if (count == 7)
        {
            Start();
            transform.position = new Vector3(100f, gameObject.transform.position.y, 3.0f);
        }
    }
    void SetAllText()
    {
        countText.text = "Count: " + count.ToString();
        livesText.text = "Lives: " + lives.ToString();
        if (count >= 14)
        {
            winText.text = "You Win!";
        }
        else if (lives == 0)
        {
            Destroy(gameObject);
            loseText.text = "You Lose";
        }

    }
    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    private bool IsGrounded()
    {
        if (rb2d.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        if (!isGrounded)
        {
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }

    private void HandleMovement(float horizontal)
    {
        if(rb2d.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }
    }
}
