using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemInteractionBrain : MonoBehaviour
{
    public static Interaction[] InteractionsStatic;
    public List<Interaction> Interactions = new();
    private ItemInteractionAssingmentLoad IIAL;


    private void Awake()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            return;
        }
        IIAL = FindObjectOfType<ItemInteractionAssingmentLoad>();
    }
    private void Start()
    {
        InteractionsStatic = Interactions.ToArray();
    }
    private void Update()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            return;
        }
        List<Interaction> tmpInteractions = new();
        for (int i = 0; i < IIAL.ItemToObjectsAssingments.Length; i++)
        {
            for (int j = 0; j < IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].InteractWithObject)
                {
                    Interaction interaction = new()
                    {
                        Item = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.Item,
                        ObjectToInteract = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].Object
                    };
                    tmpInteractions.Add(interaction);

                }
            }
        }
        
        
        if (Interactions.Count > 0 && Interactions.Count <= tmpInteractions.Count)
        {
            for (int i = 0; i < Interactions.Count; i++)
            {
                if (Interactions[i].Item == null || Interactions[i].ObjectToInteract == null)
                {
                    Interactions.RemoveAt(i);
                }
                if (tmpInteractions[i].Item == Interactions[i].Item && tmpInteractions[i].ObjectToInteract == Interactions[i].ObjectToInteract)
                {
                    continue;
                }
                Interactions.Add(tmpInteractions[i]);
            }
        }
        else
        {
            Interactions = tmpInteractions;
        }
    }
}

[Serializable]
public class Interaction
{
    public GameObject Item;
    public GameObject ObjectToInteract;
    public int InteractionDialogueID;
    public bool OpensDoor;

}
