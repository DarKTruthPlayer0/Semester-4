using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMoveServant : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CamMoveV3 moveV3;
    
    [SerializeField] private Directions direction;
    private bool holdingMouse;

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Mouse Down");
        holdingMouse = true;
        SetMoveDirection();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("Mouse UP");
        holdingMouse = false;
        SetMoveDirection();
    }

    private void SetMoveDirection()
    {
        switch (direction)
        {
            case Directions.Left:
                moveV3.MoveToDirection = holdingMouse;
                moveV3.MoveDirection = Directions.Left;
                break;
            case Directions.Right:
                moveV3.MoveToDirection = holdingMouse;
                moveV3.MoveDirection = Directions.Right;
                break;
            case Directions.Up:
                moveV3.MoveToDirection = holdingMouse;
                moveV3.MoveDirection = Directions.Up;
                break;
            case Directions.Down:
                moveV3.MoveToDirection = holdingMouse;
                moveV3.MoveDirection = Directions.Down;
                break;
        }
    }
}
