using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public int defaultPositionX = -1;

    public GameManager gameManager;
    public Serializer ser;

    [HideInInspector]
    public float xPosition;

    public bool touchInput = true;

    private void Start()
    {
        xPosition = defaultPositionX;
        StartCoroutine(Freeze(1.5f, true));
    }

    void Update()
    {
        if (gameManager.gameLost) return;

        if (Input.GetKey(KeyCode.Mouse0))
            touchInput = false;

        if (Input.touchCount == 0 && !touchInput)
        {
            xPosition = Mathf.Clamp(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x), -2, 2);
        }
        else
        {
            touchInput = true;
        }


        if (transform.position.x == xPosition) return;

        float distance = Mathf.Abs(xPosition - transform.position.x);

        if (transform.position.x < xPosition)
            MovePlayer(-speed * distance * Time.deltaTime, distance);
        else
            MovePlayer(speed * distance * Time.deltaTime, distance);

    }

    void MovePlayer(float moveX, float distance = 1)
    {
        if(distance < Mathf.Abs(moveX))
        {
            transform.position = Vector3.right * xPosition;
            ser.data.DistanceTravelled += Mathf.Abs(distance);
        }
        else
        {
            transform.position = Vector3.right * (transform.position.x - moveX);
            ser.data.DistanceTravelled += Mathf.Abs(moveX);
        }
    }


    IEnumerator Freeze(float seconds, bool resetPos = false)
    {
        float speedBeforeFreezing = speed;
        speed = 0;
        yield return new WaitForSeconds(seconds);
        speed = speedBeforeFreezing;
        if (resetPos)
        {
            xPosition = defaultPositionX;
        }
    }
}
