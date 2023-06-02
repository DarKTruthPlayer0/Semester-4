using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class DialogueOrganizer : ListFunctionsExtension
{
    [Header("Tags")]
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagItems;
    [SerializeField] private string tagInteractables;


    public static DialogueClient[] DialogueClientsStatic;
    public static Dialogue[] DialoguesStatic;
    public List<Dialogue> dialogueList;
    [SerializeField] List<DialogueClient> dialogueClientsList;

    private bool helperBool2;
    private bool helperBool3;


    private void Start()
    {
        DialogueClientsStatic = dialogueClientsList.ToArray();
    }
    private void Update()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            return;
        }
        AssingDialogueClients();
    }

    private void AssingDialogueClients()
    {
        GameObject[] tempTalkableNPCGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tempItemGOs = GameObject.FindGameObjectsWithTag(tagItems);
        GameObject[] tempInteractableGOs = GameObject.FindGameObjectsWithTag(tagInteractables);
        List<GameObject> tempDialogueClientGOs = new();
        for (int i = 0; i < tempTalkableNPCGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempTalkableNPCGOs[i]);
        }
        for (int i = 0; i < tempItemGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempItemGOs[i]);
        }
        for (int i = 0; i < tempInteractableGOs.Length; i++)
        {
            tempDialogueClientGOs.Add(tempInteractableGOs[i]);
        }

        ListCompare(dialogueClientsList, tempDialogueClientGOs, () => new DialogueClient());


        List<DialogueSelect> tmpDialogueSelects = new();

        for (int i = 0; i < dialogueList.Count; i++)
        {
            Dialogue tmpDialogue = new();

            DialogueSelect dialogueSelect = new()
            {
                SelectedDialogue = tmpDialogue,
                DialogueClassification = dialogueList[i].DialogueClassification
            };
            dialogueSelect.SelectedDialogue.DialogueParts = new DialoguePart[dialogueList[i].DialogueParts.Length]; ;

            for (int j = 0; j < dialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                dialogueSelect.SelectedDialogue.DialogueParts[j] = new()
                {
                    EmotionSprite = dialogueList[i].DialogueParts[j].EmotionSprite,
                    PersonNameWhichTalks = dialogueList[i].DialogueParts[j].PersonNameWhichTalks,
                    SentenceThePersonTalk = dialogueList[i].DialogueParts[j].SentenceThePersonTalk
                };
            }
            tmpDialogueSelects.Add(dialogueSelect);
        }

        for (int i = 0; i < dialogueClientsList.Count; i++)
        {
            for (int j = 0; j < dialogueClientsList[i].DialogueSelect.Count; j++)
            {
                for (int k = 0; k < tmpDialogueSelects.Count; k++)
                {
                    if (dialogueClientsList[i].DialogueSelect[j].SelectedDialogue.DialogueClassification == tmpDialogueSelects[k].SelectedDialogue.DialogueClassification)
                    {
                        if (dialogueClientsList[i].DialogueSelect[j].DialogueClassification != tmpDialogueSelects[k].DialogueClassification)
                        {
                            dialogueClientsList[i].DialogueSelect[j].DialogueClassification = tmpDialogueSelects[k].DialogueClassification;
                        }
                        helperBool2 = true;
                        break;
                    }
                    else
                    {
                        helperBool2 = false;
                    }
                }
                if (helperBool2)
                {
                    continue;
                }
                dialogueClientsList[i].DialogueSelect.RemoveAt(j);
            }
            for (int j = 0; j < tmpDialogueSelects.Count; j++)
            {
                for (int k = 0; k < dialogueClientsList[i].DialogueSelect.Count; k++)
                {
                    if (CompareDialogueSelects(dialogueClientsList[i].DialogueSelect[k], tmpDialogueSelects[j]))
                    {
                        helperBool2 = true;
                        break;
                    }
                    else
                    {
                        helperBool2 = false;
                    }
                }
                if (helperBool2)
                {
                    continue;
                }
                DialogueSelect tmpDialogueSelect = new()
                {
                    SelectedDialogue = tmpDialogueSelects[j].SelectedDialogue,
                    DialogueClassification = tmpDialogueSelects[j].DialogueClassification
                };
                dialogueClientsList[i].DialogueSelect.Add(tmpDialogueSelect);
            }
        }
    }
    private bool CompareDialogueSelects(DialogueSelect DialogueClientsDialogueSelect, DialogueSelect tmpDialogueSlectsDialogueSelect)
    {
        helperBool3 = false;
        int sentenceCheckInt = 0;
        int personNameCheckInt = 0;

        for (int i = 0; i < tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts.Length; i++)
        {
            for (int j = 0; j < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].SentenceThePersonTalk.Equals(tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].SentenceThePersonTalk))
                {
                    sentenceCheckInt++;
                }
                if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].PersonNameWhichTalks.Equals(tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].PersonNameWhichTalks))
                {
                    personNameCheckInt++;
                }
            }
        }
        if (sentenceCheckInt == DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length &&
            personNameCheckInt == DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length)
        {
            helperBool3 = true;
        }

        if (!DialogueClientsDialogueSelect.DialogueClassification.Equals(tmpDialogueSlectsDialogueSelect.DialogueClassification) && helperBool3)
        {
            DialogueClientsDialogueSelect.DialogueClassification = tmpDialogueSlectsDialogueSelect.DialogueClassification;
            DialogueClientsDialogueSelect.SelectedDialogue.DialogueClassification = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueClassification;
        }

        for (int i = 0; i < tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts.Length; i++)
        {
            for (int j = 0; j < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                if (!DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].EmotionSprite.Equals(tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].EmotionSprite) && helperBool3)
                {
                    DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].EmotionSprite = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].EmotionSprite;
                }
            }
        }

        return helperBool3;
    }
}

[Serializable]
public class DialogueClient : Translate
{
    public GameObject GOReference;
    public List<DialogueSelect> DialogueSelect;

    public GameObject GOTranslate
    {
        get { return GOReference; }
        set { GOReference = value; }
    }
}

[Serializable]
public class DialogueSelect
{
    [HideInInspector] public string DialogueClassification;
    public bool UseThisDialogue;
     public Dialogue SelectedDialogue;
    [HideInInspector] public bool DialogueSpoken;
}

[Serializable]
public class Dialogue
{
    public string DialogueClassification;
    public DialoguePart[] DialogueParts;
}

[Serializable]
public class DialoguePart
{
    public Sprite EmotionSprite;
    public string PersonNameWhichTalks;
    public string SentenceThePersonTalk;
}
