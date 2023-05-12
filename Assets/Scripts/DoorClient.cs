using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClient : MonoBehaviour
{
    [SerializeField] private string tagCam;
    [SerializeField] private GameObject camToActivate;
    private GameObject[] tmpCamGOs;

    private void OnMouseDown()
    {
        ChangeCam();
    }
    private void ChangeCam()
    {
        for (int i = 0; i < tmpCamGOs.Length; i++)
        {
            if (tmpCamGOs[i] == camToActivate)
            {
                camToActivate.SetActive(true);
                continue;
            }
            tmpCamGOs[i].SetActive(false);
        }
    }

    private void Start()
    {
        tmpCamGOs = GameObject.FindGameObjectsWithTag(tagCam);
    }
}
