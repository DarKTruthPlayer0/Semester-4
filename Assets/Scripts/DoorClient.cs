using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClient : MonoBehaviour
{
    [SerializeField] private string tagCam;
    [SerializeField] private GameObject camToActivate;
    private GameObject[] tmpCamGOs;
    private bool helperBool;

    public void MouseDown()
    {
        ChangeCam();
    }
    private void ChangeCam()
    {
        for (int i = 0; i < RoomOrganizer.RoomsStatic.Length; i++)
        {
            if (RoomOrganizer.RoomsStatic[i].RoomGO == camToActivate.transform.parent.gameObject && RoomOrganizer.RoomsStatic[i].IsLocked)
            {
                helperBool = true;
                break;
            }
            else
            {
                helperBool = false;
            }
        }
        if (helperBool)
        {
            return;
        }

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
