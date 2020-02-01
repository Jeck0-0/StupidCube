using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.5f;
    //  public float health = 2;
    

    private int verticalDirection = 1;
    private bool canMove = true;
    public bool gavePoint = false;

    public GameManager gameManager;

    void Start()
    {
        // Decide wether to go up or down
        if (transform.position.y > 0)
            verticalDirection = -1;

        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    { 
        // Move
        if(canMove)
            transform.Translate(Vector3.up * verticalDirection * speed * Time.deltaTime);

        // Add Score
        if (gameManager != null && !gavePoint && !gameManager.gameLost && canGivePoint())
        {
            gameManager.AddScore();
            gavePoint = true;
        }
        

        // Destroy
        if (verticalDirection<0 && transform.position.y < -7)
        {
            Destroy(gameObject);
        }
        else if(verticalDirection > 0 && transform.position.y > 7)
        {
            Destroy(gameObject);
        }
    }
    
    public void OnContinuePlaying()
    {
        canMove = false;
        Destroy(gameObject, 1);
    }

    public bool canGivePoint()
    {
        bool canGive = false;

        if (transform.position.y <= 0 && verticalDirection < 0)
        { canGive = true; }
        else if (transform.position.y >= 0 && verticalDirection > 0)
        { canGive = true; }

        return canGive;
    }
    
    public void hitPlayer()
    {
        canMove = false;
    }
}
