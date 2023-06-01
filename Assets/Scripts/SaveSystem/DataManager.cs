using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [HideInInspector] public SaveDataContainer SaveDataContainer;

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
        
        
        SaveDataContainer.InventorySave = PlayerBrain.Inventory;
        SaveDataContainer.RoomSave = RoomOrganizer.RoomsStatic;
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

        PlayerBrain.Inventory = SaveDataContainer.InventorySave;
        RoomOrganizer.RoomsStatic = SaveDataContainer.RoomSave;
        GameBrainScript.Styles = SaveDataContainer.StylesSave.ToList();

        Debug.Log("SyncDatasuc!");
    }
}

[Serializable]
public class SaveDataContainer
{
    //public DialogueClient[] DialogueClientsSave;
    public List<DialogueSelect[]> DialogueClientsDialogueSelectSave;
    
    //public Interaction[] InteractionsSave;
    public List<Paths[]> InteractionsPathsSave;
    
    public Inventory InventorySave;
    public Room[] RoomSave;
    public GameBrainScript.Style[] StylesSave;

}