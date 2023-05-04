using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ItemInteractionBrain : MonoBehaviour
{
    public List<Interaction> Interactions;
    private ItemInteractionAssingmentLoad IIAL;

    private void Update()
    {
        List<Interaction> tmpInteractions = new();
        for (int i = 0; i < IIAL.ItemToObjectsAssingments.Length; i++)
        {
            for (int j = 0; j < IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (!IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].InteractWithObject)
                {
                    continue;
                }
                Interaction interaction = new()
                {
                    Item = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.Item,
                    ObjectToInteract = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].Object
                };
                tmpInteractions.Add(interaction);
            }
        }
        if (Interactions.Count > 0)
        {
            for (int i = 0; i < Interactions.Count; i++)
            {
                if (Interactions[i].Item == null || Interactions[i].ObjectToInteract == null)
                {
                    Interactions.RemoveAt(i);
                }
            }

            for (int i = 0; i < Interactions.Count; i++)
            {
                for (int j = 0; j < tmpInteractions.Count; j++)
                {
                    if (tmpInteractions[j].Item == Interactions[i].Item && tmpInteractions[j].ObjectToInteract == Interactions[i].ObjectToInteract)
                    {
                        continue;
                    }
                    Interactions.Add(tmpInteractions[j]);
                }
            }
        }
        else
        {
            Interactions = tmpInteractions;
        }
    }

    private void Awake()
    {
        IIAL = FindObjectOfType<ItemInteractionAssingmentLoad>();
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
