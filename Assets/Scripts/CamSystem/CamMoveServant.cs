using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamMoveServant : MonoBehaviour
{
    public CamMoveTranslate camMoveTranslate;
    
    [SerializeField] private Directions direction;
    private bool holdingMouse;

    public void ArrowDown()
    {
        holdingMouse = true;
        SetMoveDirection();
    }

    public void ArrowUp()
    {
        holdingMouse = false;
        SetMoveDirection();
    }

    private void SetMoveDirection()
    {
        switch (direction)
        {
            case Directions.Left:
                camMoveTranslate.SetCamMove(holdingMouse, Directions.Left);
                break;
            case Directions.Right:
                camMoveTranslate.SetCamMove(holdingMouse, Directions.Right);
                break;
            case Directions.Up:
                camMoveTranslate.SetCamMove(holdingMouse, Directions.Up);
                break;
            case Directions.Down:
                camMoveTranslate.SetCamMove(holdingMouse, Directions.Down);
                break;
        }
    }

    private void Start()
    {
        camMoveTranslate = FindObjectOfType<CamMoveTranslate>();
    }
}
