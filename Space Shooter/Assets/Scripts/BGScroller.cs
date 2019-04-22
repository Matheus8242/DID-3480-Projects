using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;

    private Vector3 currentPosition;
    private Vector3 startPosition;
    private GameController gameController;

    void Start()
    {
        startPosition = transform.position;

        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if(gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if(gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script.");
        }
    }

    void Update()
    {
        currentPosition = transform.position;

        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
        transform.position = startPosition + Vector3.forward * newPosition;
    }

    private void FixedUpdate()
    {
        currentPosition = transform.position;
        if (gameController.gameOver == true)
        {
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
            scrollSpeed = -5;
            transform.position = currentPosition;
        }
    }
}