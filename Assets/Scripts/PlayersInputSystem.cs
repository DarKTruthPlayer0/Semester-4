using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayersInputSystem : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectUI;

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
            // Ein UI-Element wurde getroffen
            // Führe die entsprechende Aktion aus
            print(results[0].gameObject);
        }
        else
        {
            // Überprüfe, ob ein 2D- oder 3D-Objekt getroffen wurde
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Ein 3D-Objekt wurde getroffen
                // Führe die entsprechende Aktion aus
                print(hit.collider.gameObject);
            }
            else if (hit2D.collider != null)
            {
                // Ein 2D-Objekt wurde getroffen
                // Führe die entsprechende Aktion aus
                print(hit2D.collider.gameObject);
            }
            else
            {
                // Kein Objekt wurde getroffen
                // Führe die entsprechende Aktion aus
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckPlayerInput();
        }
    }
}
