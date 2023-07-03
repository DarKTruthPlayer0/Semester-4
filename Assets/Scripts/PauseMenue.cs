using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenue : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenue;
    [SerializeField] private GameObject optionsMenue;

    public void Return()
    {
        pauseMenue.SetActive(false);
    }

    public void Options()
    {
        optionsMenue.SetActive(false);
    }

    public void Back(GameObject screenToClose)
    {
        screenToClose.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenue.SetActive(true);
        }
    }
}
