using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

                for (int y = 0; y < tmpGOs.Length; y++)
                {
                    if (y < ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Count)
                    {
                        continue;
                    }
                    ItemObjectsInteractionAssingment tmpIOTA = new();
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Add(tmpIOTA);
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects[y].Object = tmpGOs[y];
                }


                for (int i = 0; i < ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Count; i++)
                {
                    if (ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects[i].Object != tmpGOs[i])
                    {
                        ItemObjectsInteractionAssingment tmpIOIA = new()
                        {
                            Object = tmpGOs[i]
                        };
                        ItemToObjectsAssingments[x].ItemMatchingInteractionObjects.InteractionObjects.Add(tmpIOIA);
                    }
                };
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
