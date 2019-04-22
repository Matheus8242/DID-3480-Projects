using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text uiText;
    public float mainTimer;

    private float timer;
    private bool canCount = true;
    private bool doOnce = false;

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public AudioClip gameOverSong;
    public AudioClip victorySong;

    public AudioSource musicSource;

    public Text ScoreText;
    public Text restartText;
    public Text gameOverText;
    public Text winText;
    public Text creditsText;
    public Text livesText;

    public bool gameOver;
    private bool restart;
    public bool isDead;
    private int score;
    private int lives;

    void Start()
    {
        timer = mainTimer;

        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        winText.text = "";
        creditsText.text = "";

        score = 0;
        lives = 3;

        UpdateLives();
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press the 'Space' key to restart.";
                restart = true;
                break;
            }
        }
    }

    void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            uiText.text = timer.ToString("F");
        }
        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;

            if (timer <= 0)
            {
                isDead = true;
                GameOver();
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Main");
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        if (score >= 100)
        {
            musicSource.clip = victorySong;
            musicSource.Play();
            winText.text = "You win!";
            creditsText.text = "Game Created by Matheus Campos.";
            gameOver = true;
            restart = true;
        }
    }

    public void GameOver()
    {
        musicSource.clip = gameOverSong;
        musicSource.Play();
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void SubLive()
    {
        lives--;
        UpdateLives();
        if (lives == 0)
        {
            isDead = true;
            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            lives = lives - 1;
            UpdateLives();
        }
    }
    void UpdateLives()
    {
        livesText.text = "Lives: " + lives.ToString();
    }
}