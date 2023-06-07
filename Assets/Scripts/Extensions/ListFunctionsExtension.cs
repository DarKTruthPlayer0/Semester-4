using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListFunctionsExtension : MonoBehaviour
{
    private bool helperBool;

    /*
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
    */
    public void ListCompare<T>(List<T> ExistingList, List<GameObject> GOCompareList, Func<T> createItem) where T : ITranslate
    {
        var goCompareSet = new HashSet<GameObject>(GOCompareList);
        var existingSet = new HashSet<GameObject>(ExistingList.Select(x => x.GOTranslate));

        for (int i = ExistingList.Count - 1; i >= 0; i--)
        {
            if (!goCompareSet.Contains(ExistingList[i].GOTranslate))
            {
                ExistingList.RemoveAt(i);
            }
        }

        for (int i = 0; i < GOCompareList.Count; i++)
        {
            if (!existingSet.Contains(GOCompareList[i]))
            {
                T newItem = createItem();
                newItem.GOTranslate = GOCompareList[i];
                ExistingList.Add(newItem);
            }
        }
    }


    public void ListCompareListsUseSameGOs<T>(List<T> ExistingList, List<GameObject> GOCompareList, GameObject CompareObject, Func<T> createItem) where T : ITranslate
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
            if (helperBool || GOCompareList[i] == CompareObject)
            {
                continue;
            }
            T newItem = createItem();
            newItem.GOTranslate = GOCompareList[i];
            ExistingList.Add(newItem);
        }
    }
}

public interface ITranslate
{
    GameObject GOTranslate { get; set; }
}

public class Torte : ITranslate
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