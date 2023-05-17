using System;
using System.Collections.Generic;
using UnityEngine;

public class ListFunctionsExtension : MonoBehaviour
{
    private bool helperBool;

    public void ListCompare<T>(List<T> ExistingList, List<GameObject> GOCompareList, Func<T> createItem) where T : Translate
    {
        for (int i = 0; i < ExistingList.Count; i++)
        {
            for (int j = 0; j < GOCompareList.Count; j++)
            {
                if (ExistingList[i].GOTranslate == GOCompareList[j])
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

        for (int i = 0; i < GOCompareList.Count; i++)
        {
            for (int j = 0; j < ExistingList.Count; j++)
            {
                if (GOCompareList[i] == ExistingList[j].GOTranslate)
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
            newItem.GOTranslate = GOCompareList[i];
            ExistingList.Add(newItem);
        }
    }

    public void ListCompareListsUseSameGOs<T>(List<T> ExistingList, List<GameObject> GOCompareList, Func<T> createItem) where T : Translate
    {
        for (int i = 0; i < ExistingList.Count; i++)
        {
            for (int j = 0; j < GOCompareList.Count; j++)
            {
                if (ExistingList[i].GOTranslate == GOCompareList[j])
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

        for (int i = 0; i < GOCompareList.Count; i++)
        {
            for (int j = 0; j < ExistingList.Count; j++)
            {
                if (ExistingList[j].GOTranslate == GOCompareList[i])
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
            newItem.GOTranslate = GOCompareList[i];
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