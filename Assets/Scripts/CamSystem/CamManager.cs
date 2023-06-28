using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [SerializeField] private string tagCamHolder;

    [SerializeField] private GameObject startingRoomCamHolder;
    private GameObject[] CamHolders;

    private void SetStartRoomCam()
    {
        PlayersInputSystem.ActiveCam = startingRoomCamHolder.transform.GetComponentInChildren<Camera>();
    }

    public void SetNewActiveCam(GameObject newCamHolder)
    {
        PlayersInputSystem.ActiveCam = newCamHolder.GetComponentInChildren<Camera>();

        for (int i = 0; i < CamHolders.Length; i++)
        {
            if (CamHolders[i] == newCamHolder)
            {
                newCamHolder.SetActive(true);
                continue;
            }
            CamHolders[i].SetActive(false);
        }
    }

    private void Start()
    {
        SetStartRoomCam();

        CamHolders = GameObject.FindGameObjectsWithTag(tagCamHolder);

        for (int i = 0; i < CamHolders.Length; i++)
        {
            if (CamHolders[i] == startingRoomCamHolder)
            {
                continue;
            }
            CamHolders[i].SetActive(false);
        }
    }
}
