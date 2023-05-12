using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomOrganizer : MonoBehaviour
{
    public static Room[] RoomsStatic;

    [SerializeField] private List<Room> rooms;
    [SerializeField] private string tagRoom;
    private bool sortHelpBool;

    private void RefreshRoomList()
    {
        GameObject[] tmpRoomGOs = GameObject.FindGameObjectsWithTag(tagRoom);

        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < tmpRoomGOs.Length; j++)
            {
                if (rooms[i].RoomGO == tmpRoomGOs[j])
                {
                    sortHelpBool = true;
                    break;
                }
                else
                {
                    sortHelpBool = false;
                }
            }
            if (sortHelpBool)
            {
                continue;
            }
            rooms.RemoveAt(i);
        }
        for (int j = 0; j < tmpRoomGOs.Length; j++)
        {
            for (int k = 0; k < rooms.Count; k++)
            {
                if (rooms[k].RoomGO == tmpRoomGOs[j] && tmpRoomGOs[k])
                {
                    sortHelpBool = true;
                    break;
                }
                else
                {
                    sortHelpBool = false;
                }
            }
            if (sortHelpBool)
            {
                continue;
            }
            Room tmpRoomsGO = new()
            {
                RoomGO = tmpRoomGOs[j]
            };
            rooms.Add(tmpRoomsGO);
        }
    }

    private void Start()
    {
        RoomsStatic = rooms.ToArray();
    }

    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        RefreshRoomList();
    }
}

[Serializable]
public class Room
{
    public GameObject RoomGO;
    public bool IsLocked;
}