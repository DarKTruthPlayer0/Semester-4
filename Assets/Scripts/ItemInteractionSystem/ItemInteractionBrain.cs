using System;
using System.Collections.Generic;
using System.Linq;
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
    
    private bool sentenceCheckBool;
    private bool personNameCheckBool;

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
                    interaction.InteractionName = interaction.Item.name + " interacts with " + interaction.Interactable.name;
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
                        if (tmpInteractions[i].InteractionName != Interactions[j].InteractionName)
                        {
                            Interactions[j].InteractionName = tmpInteractions[i].InteractionName;
                        }
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
                DialogueClassification = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueClassification
            };
            dialogueSelect.SelectedDialogue.DialogueClassification = dialogueSelect.DialogueClassification;
            dialogueSelect.SelectedDialogue.DialogueParts = new DialoguePart[dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts.Length]; ;

            for (int j = 0; j < dialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                dialogueSelect.SelectedDialogue.DialogueParts[j] = new()
                {
                    Emotions = new Emotion[dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].Emotions.Length],
                    PersonNameWhichTalks = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].PersonNameWhichTalks,
                    SentenceThePersonTalk = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].SentenceThePersonTalk
                };
                for (int k = 0; k < dialogueSelect.SelectedDialogue.DialogueParts[j].Emotions.Length; k++)
                {
                    dialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k] = new()
                    {
                        EmotionSprite = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].EmotionSprite,
                        EmotionSound = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].EmotionSound,
                        Style = dialogueOrganizer.DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].Style
                    };
                }
            }
            tmpDialogueSelects.Add(dialogueSelect);
        }

        for (int i = 0; i < Interactions.Count; i++)
        {
            var tmpDialogueSelectsSet = new HashSet<DialogueSelect>(tmpDialogueSelects);

            for (int j = 0; j < Interactions[i].Paths.Length; j++)
            {
                var InteractionPath = Interactions[i].Paths[j];
                for (int k = InteractionPath.DialogueSelect.Count - 1; k >= 0; k--)
                {
                    var dialogueSelect = InteractionPath.DialogueSelect[k];
                    if (!tmpDialogueSelectsSet.Any(tmpDialogueSelect => CompareDialogueSelects(dialogueSelect, tmpDialogueSelect)))
                    {
                        InteractionPath.DialogueSelect.RemoveAt(k);
                    }
                }

                foreach (var tmpDialogueSelect in tmpDialogueSelects)
                {
                    if (!InteractionPath.DialogueSelect.Any(ds => CompareDialogueSelects(ds, tmpDialogueSelect)))
                    {
                        DialogueSelect newDialogueSelect = new()
                        {
                            SelectedDialogue = tmpDialogueSelect.SelectedDialogue,
                            DialogueClassification = tmpDialogueSelect.DialogueClassification
                        };
                        InteractionPath.DialogueSelect.Add(newDialogueSelect);
                    }
                }
                // Synchronisiere die Emotions-Arrays
                for (int k = 0; k < InteractionPath.DialogueSelect.Count; k++)
                {
                    var compareDialogueSelect = InteractionPath.DialogueSelect[k].SelectedDialogue;
                    for (int l = 0; l < tmpDialogueSelects.Count; l++)
                    {
                        var tmpDialogueSelect = tmpDialogueSelects[l].SelectedDialogue;
                        for (int m = 0; m < compareDialogueSelect.DialogueParts.Length; m++)
                        {
                            if (tmpDialogueSelect.DialogueClassification != compareDialogueSelect.DialogueClassification)
                            {
                                continue;
                            }
                            compareDialogueSelect.DialogueParts[m].Emotions = new Emotion[tmpDialogueSelect.DialogueParts[m].Emotions.Length];
                            for (int n = 0; n < compareDialogueSelect.DialogueParts[m].Emotions.Length; n++)
                            {
                                compareDialogueSelect.DialogueParts[m].Emotions[n] = new();
                                if (tmpDialogueSelect.DialogueParts[m].Emotions[n].EmotionSprite != null)
                                {
                                    compareDialogueSelect.DialogueParts[m].Emotions[n].EmotionSprite = tmpDialogueSelect.DialogueParts[m].Emotions[n].EmotionSprite;
                                }
                                if (tmpDialogueSelect.DialogueParts[m].Emotions[n].EmotionSound != null)
                                {
                                    compareDialogueSelect.DialogueParts[m].Emotions[n].EmotionSound = tmpDialogueSelect.DialogueParts[m].Emotions[n].EmotionSound;
                                }
                                compareDialogueSelect.DialogueParts[m].Emotions[n].Style = tmpDialogueSelect.DialogueParts[m].Emotions[n].Style;
                            }
                        }
                    }
                }
            }
        }
    }

    private bool CompareDialogueSelects(DialogueSelect DialogueClientsDialogueSelect, DialogueSelect tmpDialogueSelectsDialogueSelect)
    {
        int count = 0;

        for (int i = 0; i < tmpDialogueSelectsDialogueSelect.SelectedDialogue.DialogueParts.Length; i++)
        {
            for (int j = 0; j < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                sentenceCheckBool = false;
                personNameCheckBool = false;
                if (string.Equals(DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].SentenceThePersonTalk, tmpDialogueSelectsDialogueSelect.SelectedDialogue.DialogueParts[i].SentenceThePersonTalk))
                {
                    sentenceCheckBool = true;
                }
                if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].PersonNameWhichTalks == tmpDialogueSelectsDialogueSelect.SelectedDialogue.DialogueParts[i].PersonNameWhichTalks)
                {
                    personNameCheckBool = true;
                }
                if (sentenceCheckBool && personNameCheckBool)
                {
                    count++;
                }
            }
        }

        if (count != DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length)
        {
            return false;
        }
        //Angleiches der DialogueClassification
        if (DialogueClientsDialogueSelect.DialogueClassification != tmpDialogueSelectsDialogueSelect.DialogueClassification)
        {
            DialogueClientsDialogueSelect.DialogueClassification = tmpDialogueSelectsDialogueSelect.DialogueClassification;
            DialogueClientsDialogueSelect.SelectedDialogue.DialogueClassification = tmpDialogueSelectsDialogueSelect.SelectedDialogue.DialogueClassification;
        }

        return true;
    }
}

[Serializable]
public class Interaction
{
    [HideInInspector] public string InteractionName;
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