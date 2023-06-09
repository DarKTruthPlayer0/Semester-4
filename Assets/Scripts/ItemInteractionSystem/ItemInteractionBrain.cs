using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class ItemInteractionBrain : MonoBehaviour
{
    [Header("Essential")]
    [SerializeField] private ItemInteractionAssingmentLoad IIAL;

    [Header("Interactions")]
    public static Interaction[] InteractionsStatic;
    public List<Interaction> Interactions = new();

    private bool interactionExist;
    private bool tmpInteractionExist;
    private bool helperBool;

    private void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }
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
            for (int j = 0; j < IIAL.ItemToObjectsAssingments[i].InteractionObjects.Count; j++)
            {
                if (IIAL.ItemToObjectsAssingments[i].InteractionObjects[j].InteractWithObject)
                {
                    Interaction interaction = new()
                    {
                        Item = IIAL.ItemToObjectsAssingments[i].Item,
                        Interactable = IIAL.ItemToObjectsAssingments[i].InteractionObjects[j].Object
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
                Interactions.Add(tmpInteractions[i]);
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
                Interactions.RemoveAt(i);
            }
        }
        else
        {
            Interactions = tmpInteractions;
        }
    }

    public void SetDialogueSelect()
    {
        DialogueOrganizer dialogueOrganizer = FindObjectOfType<DialogueOrganizer>();
        List<DialogueSelect> tmpDialogueSelects = new();

        for (int i = 0; i < dialogueOrganizer.DialogueListSO.dialogueList.Count; i++)
        {
            DialogueSelect dialogueSelect = new()
            {
                SelectedDialogue = dialogueOrganizer.DialogueListSO.dialogueList[i],
                DialogueClassification = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueClassification
            };
            tmpDialogueSelects.Add(dialogueSelect);
        }

        for (int i = 0; i < Interactions.Count; i++)
        {
            for (int j = 0; j < Interactions[i].Paths.Length; j++)
            {
                for (int k = 0; k < Interactions[i].Paths[j].DialogueSelect.Count; k++)
                {
                    for (int l = 0; l < tmpDialogueSelects.Count; l++)
                    {
                        if (Interactions[i].Paths[j].DialogueSelect[k].SelectedDialogue == tmpDialogueSelects[l].SelectedDialogue)
                        {
                            if (Interactions[i].Paths[j].DialogueSelect[k].DialogueClassification != tmpDialogueSelects[l].DialogueClassification)
                            {
                                Interactions[i].Paths[j].DialogueSelect[k].DialogueClassification = tmpDialogueSelects[l].DialogueClassification;
                            }
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
                    Interactions[i].Paths[j].DialogueSelect.RemoveAt(j);
                }
            }
            for (int j = 0; j < tmpDialogueSelects.Count; j++)
            {
                for (int k = 0; k < Interactions[i].Paths.Length; k++)
                {
                    for (int l = 0; l < Interactions[i].Paths[k].DialogueSelect.Count; l++)
                    {
                        if (tmpDialogueSelects[j].SelectedDialogue == Interactions[i].Paths[k].DialogueSelect[l].SelectedDialogue)
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
                    DialogueSelect tmpDialogueSelect = new()
                    {
                        SelectedDialogue = tmpDialogueSelects[j].SelectedDialogue,
                        DialogueClassification = tmpDialogueSelects[j].DialogueClassification
                    };
                    Interactions[i].Paths[k].DialogueSelect.Add(tmpDialogueSelect);
                }
            }
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
    public List<DialogueSelect> DialogueSelect = new();
}