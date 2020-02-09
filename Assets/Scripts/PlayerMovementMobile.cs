using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementMobile : MonoBehaviour
{

    public PlayerMovement playerMovement;
    public GameManager gameManager;

    bool lastTouchWasPause = false;

    void Update()
    {
        if (gameManager.gameLost) return;

        if (Input.touchCount > 0)
        {
            
            if (IsPointerOverUIObject() || lastTouchWasPause)
            {
                lastTouchWasPause = true;
                return;
            }

            if (gameManager.paused)
            {
                gameManager.Unpause();
                lastTouchWasPause = false;
            }

            playerMovement.xPosition = Mathf.Clamp(Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x), -(gameManager.lanes - 1) / 2, (gameManager.lanes - 1) / 2);
            return;
        }

        lastTouchWasPause = false;
        
    }

    private bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        if (EventSystem.current.currentSelectedGameObject != null)
            return true;
        
        for (int touchIndex = 0; touchIndex < Input.touchCount; touchIndex++)
        {
            Touch touch = Input.GetTouch(touchIndex);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return true;
        }

        return false;
    }

}
