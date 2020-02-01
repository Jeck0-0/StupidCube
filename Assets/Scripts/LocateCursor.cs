using UnityEngine;
using UnityEngine.EventSystems;

public class LocateCursor : MonoBehaviour//, IPointerDownHandler
{
    public PlayerMovement playerMovement;
    public GameManager gameManager;
    //public Transform GoTo;

    private void OnMouseOver()
    {
        if(!gameManager.gameLost)
            playerMovement.xPosition = transform.position.x; //GoTo.transform;
    }

    public void SetPosition()
    {
        playerMovement.xPosition = transform.position.x;
    }
}
