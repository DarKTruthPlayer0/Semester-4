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
    public static ItemToObjectsAssingmentsList[] ItemToObjectsAssingmentsStatic;
    public ItemToObjectsAssingmentsList[] ItemToObjectsAssingments;

    private GameObject[] tmpGOs;


    private void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            return;
        }
        tmpGOs = GameObject.FindGameObjectsWithTag("ObjectToInteract");
        for (int x = 0; x < ItemToObjectsAssingments.Length; x++)
        {
            ItemToObjectsAssingments[x].ItemMatchingInteractionObject = new()
            {
                InteractionObjects = new()
            };
        }
    }

    private void Start()
    {
        ItemToObjectsAssingmentsStatic = ItemToObjectsAssingments;
    }

    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            return;
        }
        for (int x = 0; x < ItemToObjectsAssingments.Length; x++)
        {
            tmpGOs = GameObject.FindGameObjectsWithTag("ObjectToInteract");
            for (int z = 0; z < ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects.Count; z++)
            {
                if (!tmpGOs.Contains(ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects[z].Object) || z >= tmpGOs.Length)
                {
                    print("f");
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects.RemoveAt(z);
                }
            }

            for (int y = 0; y < tmpGOs.Length; y++)
            {
                if (y < ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects.Count)
                {
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects[y].Object = tmpGOs[y];
                }
                else
                {
                    ItemObjectsInteractionAssingment tmpIOTA = new();
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects.Add(tmpIOTA);
                    ItemToObjectsAssingments[x].ItemMatchingInteractionObject.InteractionObjects[y].Object = tmpGOs[y];
                }
            }
        }

    }
}

[Serializable]
public class ItemToObjectsAssingmentsList
{
    public ItemToObjectsAssingments ItemMatchingInteractionObject;
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
