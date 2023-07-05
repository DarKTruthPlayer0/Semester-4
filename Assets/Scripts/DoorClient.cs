using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorClient : MonoBehaviour
{
    [SerializeField] private string tagCamHolder;
    [SerializeField] private GameObject camHolderToActivate;
    private CamManager camManager;
    private bool helperBool;

    public void MouseDown()
    {
        print("UseDoor");
        ChangeCam();
    }
    private void ChangeCam()
    {
        for (int i = 0; i < RoomOrganizer.RoomsStatic.Length; i++)
        {
            if (RoomOrganizer.RoomsStatic[i].RoomGO == camHolderToActivate.transform.parent.gameObject && RoomOrganizer.RoomsStatic[i].IsLocked)
            {
                print("kakadu");
                helperBool = true;
                break;
            }
            helperBool = false;
        }
        if (helperBool)
        {
            return;
        }
        camManager.SetNewActiveCam(camHolderToActivate);
    }

    private void Awake()
    {
        camManager = FindObjectOfType<CamManager>();
    }
}
