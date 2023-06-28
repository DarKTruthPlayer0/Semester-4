using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayersInputSystem : MonoBehaviour
{
    [SerializeField] private string tagCamMoveDirectionArrow;
    [SerializeField] private string tagItems;
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagDoor;

    [SerializeField] private GameObject gameObjectUI;
    private CamMoveServant camMoveServant;

    private void CheckPlayerInput()
    {
        GraphicRaycaster raycaster = gameObjectUI.GetComponent<GraphicRaycaster>();
        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new();
        raycaster.Raycast(pointerEventData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.CompareTag(tagCamMoveDirectionArrow))
            {
                camMoveServant = results[0].gameObject.GetComponent<CamMoveServant>();
                camMoveServant.ArrowDown();
            }
        }
        else
        {
            // Checkob 2D- oder 3D-Objekt getroffen
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            // 3D Object
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag(tagItems))
                {
                    hit.collider.gameObject.GetComponent<PlayerServant>().MouseDown();
                }
            }
            // 2D Object
            else if (hit2D.collider != null)
            {
                if (hit2D.collider.gameObject.CompareTag(tagInteractable))
                {
                    hit2D.collider.gameObject.GetComponent<PlayerServant>().MouseDown();
                }
                if (hit2D.collider.gameObject.CompareTag(tagDoor))
                {
                    hit2D.collider.gameObject.GetComponent<DoorClient>().MouseDown();
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckPlayerInput();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (camMoveServant != null)
            {
                camMoveServant.ArrowUp();
                camMoveServant = null;
            }
        }
    }
}
