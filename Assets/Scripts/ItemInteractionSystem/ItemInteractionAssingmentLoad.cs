using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ItemInteractionAssingmentLoad : ListFunctionsExtension
{
    [Header("Tags")]
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagItem;

    [Header("Assingment")]
    public static ItemToObjectsAssingment[] ItemToObjectsAssingmentsStatic;
    public List<ItemToObjectsAssingment> ItemToObjectsAssingments = new();

    private GameObject[] tmpInteractableGOs;
    private GameObject[] tmpItemGOs;

    private void Awake()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }

        tmpItemGOs = GameObject.FindGameObjectsWithTag(tagItem);
        tmpInteractableGOs = GameObject.FindGameObjectsWithTag(tagInteractable);

        ListCompare(ItemToObjectsAssingments, tmpItemGOs.ToList(), () => new ItemToObjectsAssingment());
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

            ListCompare(ItemToObjectsAssingments[i].InteractionObjects, tmpInteractableGOs.ToList(), () => new ItemObjectsInteractionAssingment());

        }

    }
}

[Serializable]
public class ItemToObjectsAssingment : Translate
{
    public GameObject Item;
    public List<ItemObjectsInteractionAssingment> InteractionObjects = new();

    public GameObject GOTranslate
    {
        get { return Item; }
        set { Item = value; }
    }
}

[Serializable]
public class ItemObjectsInteractionAssingment : Translate
{
    public GameObject Object;
    public bool InteractWithObject;

    public GameObject GOTranslate
    {
        get { return Object; }
        set { Object = value; }
    }
}
