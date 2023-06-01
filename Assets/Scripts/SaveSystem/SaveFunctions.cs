using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveFunctions : MonoBehaviour
{
    private static DataManager dataManager;
    public static bool canLoadData;

    private static string path;

    public static void Save()
    {
        dataManager.GetData();
        SaveData();
    }
    public static void Load()
    {
        if (canLoadData)
        {
            print("CantLoadData");
            return;
        }

        LoadDataIntoDataToSave();
        dataManager.SyncData();
    }

    public static void NewGame()
    {
        Save();

    }

    private static void SaveData()
    {
        BinaryFormatter formatter = new();

        FileStream stream = new(path, FileMode.Create);

        formatter.Serialize(stream, dataManager.SaveDataContainer);
        stream.Close();
    }

    private static void LoadDataIntoDataToSave()
    {
        if (File.Exists(path))
        {
            FileStream stream = new(path, FileMode.Open);


            //Hier werden alle Daten eingefügt die geladenwerden sollen
            dataManager.SaveDataContainer = new BinaryFormatter().Deserialize(stream) as SaveDataContainer;

            stream.Close();
            canLoadData = true;
        }
        else
        {
            Debug.LogError("Error: Save file not found in " + path);
            canLoadData = false;
        }
    }

    private void Awake()
    {
        path = Application.persistentDataPath + "/SaveGame.sav";
        dataManager = GetComponent<DataManager>();
    }
}
