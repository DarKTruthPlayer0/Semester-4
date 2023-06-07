using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class DataManager : ListFunctionsExtension
{
    [HideInInspector] public SaveDataContainer SaveDataContainer;

    [SerializeField] private string tagItems;
    [SerializeField] private string tagRooms;
    [SerializeField] private List<InventorySaveEnabler> inventorySaveAssing;
    [SerializeField] private List<RoomIDAssing> roomIDAssings;

    public void GetData()
    {
        //Hier werden die Daten aus dem Speicherstand aus einzelnen Scripts geladen

        //SaveDataContainer.DialogueClientsSave = DialogueOrganizer.DialogueClientsStatic;
        SaveDataContainer.DialogueClientsDialogueSelectSave = new();
        for (int i = 0; i < DialogueOrganizer.DialogueClientsStatic.Length; i++)
        {
            SaveDataContainer.DialogueClientsDialogueSelectSave.Add(DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect.ToArray());
        }

        //SaveDataContainer.InteractionsSave = ItemInteractionBrain.InteractionsStatic;
        SaveDataContainer.InteractionsPathsSave = new();
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic.Length; i++)
        {
            SaveDataContainer.InteractionsPathsSave.Add(ItemInteractionBrain.InteractionsStatic[i].Paths);
        }

        //SaveDataContainer.InventorySave = PlayerBrain.Inventory;
        for (int i = 0; i < PlayerBrain.Inventory.Items.Length; i++)
        {
            if (PlayerBrain.Inventory.Items[i] != null)
            {
                continue;
            }
            for (int j = 0; j < inventorySaveAssing.Count; j++)
            {
                if (PlayerBrain.Inventory.Items[i].ItemGO == inventorySaveAssing[j].Item)
                {
                    SaveDataContainer.InventorySaveIDSave.Add(inventorySaveAssing[j].SaveID);
                }
            }
        }

        //SaveDataContainer.RoomSave = RoomOrganizer.RoomsStatic;
        for (int i = 0; i < RoomOrganizer.RoomsStatic.Length; i++)
        {
            for (int j = 0; j < roomIDAssings.Count; j++)
            {
                if (RoomOrganizer.RoomsStatic[i].RoomGO == roomIDAssings[j].Room)
                {
                    RoomSaveEnabler tmpRoomSaveEnabler = new()
                    {
                        RoomSaveID = roomIDAssings[j].SaveID,
                        Locked = RoomOrganizer.RoomsStatic[i].IsLocked
                    };
                    SaveDataContainer.RoomSaveEnablerSave.Add(tmpRoomSaveEnabler);
                    break;
                }
            }
        }

        SaveDataContainer.StylesSave = GameBrainScript.Styles.ToArray();

        Debug.Log("LoadDatasuc!");
    }

    public void SyncData()
    {
        //Hier werden die Daten aus dem Speicherstand in die einzelnen Scripts geladen

        //DialogueOrganizer.DialogueClientsStatic = SaveDataContainer.DialogueClientsSave;
        for (int i = 0; i < DialogueOrganizer.DialogueClientsStatic.Length; i++)
        {
            DialogueOrganizer.DialogueClientsStatic[i].DialogueSelect = SaveDataContainer.DialogueClientsDialogueSelectSave[i].ToList();
        }

        //ItemInteractionBrain.InteractionsStatic = SaveDataContainer.InteractionsSave;
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic.Length; i++)
        {
            ItemInteractionBrain.InteractionsStatic[i].Paths = SaveDataContainer.InteractionsPathsSave[i];
        }

        //PlayerBrain.Inventory = SaveDataContainer.InventorySave;
        for (int i = 0; i < PlayerBrain.Inventory.Items.Length; i++)
        {
            PlayerBrain.Inventory.Items[i].ItemGO = null;
            PlayerBrain.Inventory.Items[i].ItemSelected = false;
        }
        for (int i = 0; i < SaveDataContainer.InventorySaveIDSave.Count; i++)
        {
            for (int j = 0; j < inventorySaveAssing.Count; j++)
            {
                if (SaveDataContainer.InventorySaveIDSave[i] == inventorySaveAssing[j].SaveID)
                {
                    PlayerBrain.Inventory.Items[i].ItemGO = inventorySaveAssing[j].Item;
                }
            }
        }


        //RoomOrganizer.RoomsStatic = SaveDataContainer.RoomSave;
        for (int i = 0; i < SaveDataContainer.RoomSaveEnablerSave.Count; i++)
        {
            for (int j = 0; j < roomIDAssings.Count; j++)
            {
                if (SaveDataContainer.RoomSaveEnablerSave[i].RoomSaveID != roomIDAssings[j].SaveID)
                {
                    continue;
                }
                for (int k = 0; k < RoomOrganizer.RoomsStatic.Length; k++)
                {
                    if (RoomOrganizer.RoomsStatic[k].RoomGO == roomIDAssings[j].Room)
                    {
                        RoomOrganizer.RoomsStatic[k].IsLocked = SaveDataContainer.RoomSaveEnablerSave[i].Locked;
                        break;
                    }
                }
            }
        }

        GameBrainScript.Styles = SaveDataContainer.StylesSave.ToList();

        Debug.Log("SyncDatasuc!");
    }

    private void UpdateItemSaveIDList()
    {
        GameObject[] tmpItemGOs = GameObject.FindGameObjectsWithTag(tagItems);
        ListCompare(inventorySaveAssing, tmpItemGOs.ToList(), () => new InventorySaveEnabler());
        for (int i = 0; i < inventorySaveAssing.Count; i++)
        {
            inventorySaveAssing[i].SaveID = i;
        }
    }

    private void UpdateRoomIDAssing()
    {
        GameObject[] tmpRoomGOs = GameObject.FindGameObjectsWithTag(tagRooms);
        ListCompare(roomIDAssings, tmpRoomGOs.ToList(), () => new RoomIDAssing());
        for (int i = 0; i < roomIDAssings.Count; i++)
        {
            roomIDAssings[i].SaveID = i;
        }
    }

    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        UpdateItemSaveIDList();
        UpdateRoomIDAssing();
    }
}

[Serializable]
public class SaveDataContainer
{
    //public DialogueClient[] DialogueClientsSave;
    public List<DialogueSelect[]> DialogueClientsDialogueSelectSave;
    
    //public Interaction[] InteractionsSave;
    public List<Paths[]> InteractionsPathsSave;

    //public Inventory InventorySave;
    public List<int> InventorySaveIDSave;

    //public Room[] RoomSave;
    public List<RoomSaveEnabler> RoomSaveEnablerSave;

    public GameBrainScript.Style[] StylesSave;

}

[Serializable]
public class InventorySaveEnabler : ITranslate
{
    public GameObject Item;
    public int SaveID;

    public GameObject GOTranslate
    {
        get { return Item; }
        set { Item = value; }
    }
}

[Serializable]
public class RoomIDAssing : ITranslate
{
    public GameObject Room;
    public int SaveID;

    public GameObject GOTranslate
    {
        get { return Room; }
        set { Room = value; }
    }
}

[Serializable]
public class RoomSaveEnabler
{
    public int RoomSaveID;
    public bool Locked;
}