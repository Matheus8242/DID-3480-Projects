using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;
    public Text scoreText;
    public Text loseText;
    public Text livesText;

    private Rigidbody rb;
    private int countValue;
    private static int scoreValue;
    private static int livesValue;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        countValue = 0;
        scoreValue = 0;
        livesValue = 3;
        SetAllText();
        winText.text = "";
        loseText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            countValue = countValue + 1;
            scoreValue = scoreValue + 1; 
            SetAllText();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            scoreValue = scoreValue - 1;
            livesValue = livesValue - 1;
            SetAllText();
        }
        if (countValue == 12)
        {
            transform.position = new Vector3(61f, gameObject.transform.position.y, 3.0f);
        }
    }

    void SetAllText()
    {
        countText.text = "Count: " + countValue.ToString();
        scoreText.text = "Score: " + scoreValue.ToString();
        livesText.text = "Lives: " + livesValue.ToString();
        if (countValue >= 22)
        {
            winText.text = "You win!";
        }
        if(livesValue == 0)
        {
            Destroy(gameObject);
            loseText.text = "Game Over";
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
