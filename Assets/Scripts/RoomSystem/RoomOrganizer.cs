using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOrganizer : MonoBehaviour
{
    [SerializeField] private string tagRoom;
    private Room[] rooms;

    private void Start()
    {
        GameObject[] tmpRoomGOs = GameObject.FindGameObjectsWithTag(tagRoom);
        rooms = new Room[tmpRoomGOs.Length];
        for (int i = 0; i < tmpRoomGOs.Length; i++)
        {
            rooms[i].RoomGO = tmpRoomGOs[i];
        }
    }
}

[Serializable]
public class Room
{
    public GameObject RoomGO;
    public bool IsLocked;
}