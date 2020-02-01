using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovementMobile : MonoBehaviour
{

    public PlayerMovement playerMovement;
    private bool clickedOnUI;
    void Update()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (Input.touchCount > 0 && results.Count < 0)
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Touch touch = Input.GetTouch(0);
            playerMovement.xPosition = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(touch.position).x);
        }
    }
}
