using System;
using System.Collections.Generic;
using UnityEngine;

public class ListFunctionsExtension : MonoBehaviour
{
    private bool helperBool;

    public void KuchenFunktion<T>(List<T> ExistingList, List<GameObject> CompareList, Func<T> createItem) where T : Translate
    {
        for (int i = 0; i < ExistingList.Count; i++)
        {
            for (int j = 0; j < CompareList.Count; j++)
            {
                if (ExistingList[i].GOTranslate == CompareList[j])
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
                continue;
            }
            ExistingList.RemoveAt(i);
        }

        for (int i = 0; i < CompareList.Count; i++)
        {
            for (int j = 0; j < ExistingList.Count; j++)
            {
                if (CompareList[i] == ExistingList[j].GOTranslate)
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
                continue;
            }
            T newItem = createItem();
            newItem.GOTranslate = CompareList[i];
            ExistingList.Add(newItem);
        }
    }
}

public interface Translate
{
    GameObject GOTranslate { get; set; }
}

public class Torte : Translate
{
    public GameObject TortenGO;
    public bool Existiert;
    public string TortenName;

    public GameObject GOTranslate
    {
        get { return TortenGO; }
        set { TortenGO = value; }
    }
}