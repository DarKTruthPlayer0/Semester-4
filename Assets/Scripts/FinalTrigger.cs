using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FinalTrigger : MonoBehaviour
{
    [SerializeField] private string tagFinalDoor;
    [SerializeField] private GameObject rectorateGO;
    [SerializeField] private VideoPlayer player;
    [SerializeField] private VideoClip cyberpunktOutro;
    [SerializeField] private VideoClip datingsimOutro;
    [SerializeField] private VideoClip horrorOutro;
    private bool helperBool;

    public void CheckFinalLocked()
    {
        for (int i = 0; i < RoomOrganizer.RoomsStatic.Length; i++)
        {
            if (RoomOrganizer.RoomsStatic[i].RoomGO == rectorateGO && RoomOrganizer.RoomsStatic[i].IsLocked)
            {
                helperBool = true;
                break;
            }
            helperBool = false;
        }
        if (helperBool)
        {
            return;
        }
        CheckFinalStyle();
    }

    private void CheckFinalStyle()
    {
        player.gameObject.SetActive(true);
        switch (GameBrainScript.PresentStyle)
        {
            case GameBrainScript.Style.Cyberpunk:
                player.clip = cyberpunktOutro;
                break;
            case GameBrainScript.Style.Datingsim:
                player.clip = datingsimOutro;
                break;
            case GameBrainScript.Style.Horror:
                player.clip = horrorOutro;
                break;
        }
        player.Play();
    }

    private void Start()
    {
        player = GameObject.Find("OutroHolder").GetComponent<VideoPlayer>();
        player.gameObject.SetActive(false);

        GameObject.FindGameObjectWithTag(tagFinalDoor).AddComponent<FinalTriggerClient>().FinalTriggerScript = this;
    }
}

[Serializable]
public class FinalTriggerClient : MonoBehaviour
{
    public FinalTrigger FinalTriggerScript;

    public void TriggerFinalCheck()
    {
        FinalTriggerScript.CheckFinalLocked();
    }
}