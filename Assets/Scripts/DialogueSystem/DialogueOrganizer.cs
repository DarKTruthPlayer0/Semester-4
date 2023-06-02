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

        ListCompare(dialogueClientsList, tempDialogueClientGOs, ()=> new DialogueClient());
        

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
                    if (dialogueClientsList[i].DialogueSelect[j].SelectedDialogue == tmpDialogueSelects[k].SelectedDialogue)
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
                    if (dialogueClientsList[i].DialogueSelect[k].SelectedDialogue == tmpDialogueSelects[j].SelectedDialogue)
                    {
                        helperBool2 = true;
                        print("yeah");
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
    [HideInInspector] public Dialogue SelectedDialogue;
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
