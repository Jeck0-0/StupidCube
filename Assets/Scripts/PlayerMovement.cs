using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;

    public int defaultPositionX = 0;

    public GameManager gameManager;

    [HideInInspector]
    public float xPosition;

    private void Awake()
    {
        xPosition = 0f;
    }

    void Update()
    {
        if (gameManager.gameLost) return;
        if (transform.position.x == xPosition) return;

        float distance = Mathf.Abs(xPosition - transform.position.x);

        if (transform.position.x < xPosition)
        {
            MovePlayer(-speed * distance * Time.deltaTime, distance);
        }
        else
        {
            MovePlayer(speed * distance * Time.deltaTime, distance);
        }

    }

    void MovePlayer(float moveX, float distance = 1)
    {
        if(distance < Mathf.Abs(moveX))
        {
            transform.position = Vector3.right * xPosition;
        }
        else
        {
            transform.position = Vector3.right * (transform.position.x - moveX);
        }
    }
}
