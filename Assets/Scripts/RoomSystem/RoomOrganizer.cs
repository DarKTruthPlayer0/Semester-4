using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class RoomOrganizer : ListFunctionsExtension
{
    public static Room[] RoomsStatic;

    [SerializeField] private List<Room> rooms;
    [SerializeField] private string tagRoom;

    private void RefreshRoomList()
    {
        GameObject[] tmpRoomGOs = GameObject.FindGameObjectsWithTag(tagRoom);

        ListCompare(rooms, tmpRoomGOs.ToList(), () => new Room());
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
public class Room : ITranslate
{
    public GameObject RoomGO;
    public bool IsLocked;

    public GameObject GOTranslate
    {
        get { return RoomGO; }
        set { RoomGO = value; }
    }
}