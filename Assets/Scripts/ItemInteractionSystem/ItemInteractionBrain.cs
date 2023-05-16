using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class ItemInteractionBrain : MonoBehaviour
{
    [Header ("Essential")]
    [SerializeField] private ItemInteractionAssingmentLoad IIAL;

    [Header ("Interactions")]
    public static Interaction[] InteractionsStatic;
    public List<Interaction> Interactions = new();

    private bool interactionExist;
    private bool tmpInteractionExist;

    private void Start()
    {
        InteractionsStatic = Interactions.ToArray();
    }
    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        List<Interaction> tmpInteractions = new();
        for (int i = 0; i < IIAL.ItemToObjectsAssingments.Count; i++)
        {
            for (int j = 0; j < IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].InteractWithObject)
                {
                    Interaction interaction = new()
                    {
                        Item = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.Item,
                        Interactable = IIAL.ItemToObjectsAssingments[i].ItemMatchingInteractionObject.InteractionObjects[j].Object
                    };
                    tmpInteractions.Add(interaction);

                }
            }
        }
        
        
        if (Interactions.Count > 0 && tmpInteractions.Count > 0)
        {
            for (int i = 0; i < tmpInteractions.Count; i++)
            {
                for (int j = 0; j < Interactions.Count; j++)
                {
                    if (tmpInteractions[i].Item == Interactions[j].Item && tmpInteractions[i].Interactable == Interactions[j].Interactable)
                    {
                        tmpInteractionExist = true;
                        break;
                    }
                    else
                    {
                        tmpInteractionExist = false;
                    }
                }
                if (tmpInteractionExist)
                {
                    continue;
                }
                else
                {
                    Interactions.Add(tmpInteractions[i]);
                }
            }

            for (int i = 0; i < Interactions.Count; i++)
            {
                if (Interactions[i].Item == null || Interactions[i].Interactable == null)
                {
                    Interactions.RemoveAt(i);
                }
                for (int j = 0; j < tmpInteractions.Count; j++)
                {
                    if (tmpInteractions[j].Item == Interactions[i].Item && tmpInteractions[j].Interactable == Interactions[i].Interactable)
                    {
                        interactionExist = true;
                        break;
                    }
                    else
                    {
                        interactionExist = false;
                    }
                }
                if (interactionExist)
                {
                    continue;
                }
                else
                {
                    Interactions.RemoveAt(i);
                }
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
    public GameObject Interactable;
    public Paths[] Paths;
    public GameObject RoomtoUnlock;
}

[Serializable]
public class Paths
{
    public GameBrainScript.Style Style;
    public int InteractionDialogueID;
    public Dialogues pants;
}
