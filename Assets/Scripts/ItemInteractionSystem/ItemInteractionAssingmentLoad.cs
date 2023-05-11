using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ItemInteractionAssingmentLoad : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagItem;

    [Header("Assingment")]
    public static ItemToObjectsAssingmentsList[] ItemToObjectsAssingmentsStatic;
    public List<ItemToObjectsAssingmentsList> ItemToObjectsAssingments = new();

    private GameObject[] tmpInteractableGOs;
    private GameObject[] tmpItemGOs;
    private bool itemInITOA;
    private bool iTOAItemInTmpItems;


    private void Awake()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }

        tmpItemGOs = GameObject.FindGameObjectsWithTag(tagItem);
        tmpInteractableGOs = GameObject.FindGameObjectsWithTag(tagInteractable);
        for (int i = 0; i < tmpItemGOs.Length; i++)
        {
            for (int j = 0; j < ItemToObjectsAssingments.Count; j++)
            {
                if ((ItemToObjectsAssingments[j].ItemMatchingInteractionObject.Item == tmpItemGOs[i]))
                {
                    itemInITOA = true;
                    break;
                }
                else
                {
                    itemInITOA = false;
                }
            }
            if (itemInITOA)
            {
                continue;
            }
            ItemToObjectsAssingmentsList tmpITOAL = new();
            tmpITOAL.ItemMatchingInteractionObject.Item = tmpItemGOs[i];
            ItemToObjectsAssingments.Add(tmpITOAL);
        }

        for (int i = 0; i < ItemToObjectsAssingments.Count; i++)
        {
            for (int j = 0; j < tmpItemGOs.Length; j++)
            {
                if ((ItemToObjectsAssingments[i].ItemMatchingInteractionObject.Item == tmpItemGOs[j]))
                {
                    iTOAItemInTmpItems = true;
                    break;
                }
                else
                {
                    iTOAItemInTmpItems = false;
                }
            }
            if (iTOAItemInTmpItems)
            {
                continue;
            }
            ItemToObjectsAssingments.RemoveAt(i);
        }
    }

    private void Start()
    {
        ItemToObjectsAssingmentsStatic = ItemToObjectsAssingments.ToArray();
    }

    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        for (int i = 0; i < ItemToObjectsAssingments.Count; i++)
        {
            tmpInteractableGOs = GameObject.FindGameObjectsWithTag(tagInteractable);
            for (int j = 0; j < ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (!tmpInteractableGOs.Contains(ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].Object) || j >= tmpInteractableGOs.Length)
                {
                    ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.RemoveAt(j);
                }
            }

            for (int y = 0; y < tmpInteractableGOs.Length; y++)
            {
                if (y < ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Count)
                {
                    ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[y].Object = tmpInteractableGOs[y];
                }
                else
                {
                    ItemObjectsInteractionAssingment tmpIOTA = new();
                    ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Add(tmpIOTA);
                    ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[y].Object = tmpInteractableGOs[y];
                }
            }
        }

    }
}

[Serializable]
public class ItemToObjectsAssingmentsList
{
    public ItemToObjectsAssingment ItemMatchingInteractionObject = new();
}

[Serializable]
public class ItemToObjectsAssingment
{
    public GameObject Item;
    public List<ItemObjectsInteractionAssingment> InteractionObjects = new();
}

[Serializable]
public class ItemObjectsInteractionAssingment
{
    public GameObject Object;
    public bool InteractWithObject;
}
