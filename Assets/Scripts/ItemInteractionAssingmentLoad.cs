using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ItemInteractionAssingmentLoad : MonoBehaviour
{
    public ItemToObjectsAssingmentsList[] ItemToObjectsAssingments;
    private GameObject[] tmpGOs;


    private void Awake()
    {
        tmpGOs = GameObject.FindGameObjectsWithTag("ObjectToInteract");
        for (int x = 0; x < ItemToObjectsAssingments.Length; x++)
        {
            ItemToObjectsAssingments[x].ItemMatchingInteractionObjects = new()
            {
                InteractionObjects = new()
            };
        }
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            for (int x = 0; x < ItemToObjectsAssingments.Length; x++)
            {
                tmpGOs = GameObject.FindGameObjectsWithTag("ObjectToInteract");
                for (int z = 0; z < ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Count; z++)
                {
                    if (!tmpGOs.Contains(ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects[z].Object) || z >= tmpGOs.Length)
                    {
                        print ("f");
                        ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.RemoveAt(z);
                    }
                }

                for (int y = 0; y < tmpGOs.Length; y++)
                {
                    if (y < ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Count)
                    {
                        ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects[y].Object = tmpGOs[y];
                    }
                    else
                    {
                        ItemObjectsInteractionAssingment tmpIOTA = new();
                        ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Add(tmpIOTA);
                        ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects[y].Object = tmpGOs[y];
                    }
                }
            }
        }
    }
}

[Serializable]
public class ItemToObjectsAssingmentsList
{
    public ItemToObjectsAssingments ItemMatchingInteractionObjects;
}

[Serializable]
public class ItemToObjectsAssingments
{
    public GameObject Item;
    public List<ItemObjectsInteractionAssingment> InteractionObjects;
}

[Serializable]
public class ItemObjectsInteractionAssingment
{
    public GameObject Object;
    public bool InteractWithObject;
}
