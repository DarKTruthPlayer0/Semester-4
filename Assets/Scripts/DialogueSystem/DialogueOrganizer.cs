using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class DialogueOrganizer : ListFunctionsExtension
{
    [Header("Tags")]
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagItems;
    [SerializeField] private string tagInteractables;

    [SerializeField] private DialogueListSO DialogueListSO;
    
    public static DialogueClient[] DialogueClientsStatic;
    public static Dialogue[] DialoguesStatic;
    [HideInInspector] public List<Dialogue> DialogueList;
    [SerializeField] private List<DialogueClient> dialogueClientsList;

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
        DialogueList = DialogueListSO.dialogueList;
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
    }

    public void SyncDialoguesInDialogueClients()
    {
        List<DialogueSelect> tmpDialogueSelects = new();

        for (int i = 0; i < DialogueList.Count; i++)
        {
            Dialogue tmpDialogue = new();

            DialogueSelect dialogueSelect = new()
            {
                SelectedDialogue = tmpDialogue,
                DialogueClassification = DialogueList[i].DialogueClassification
            };
            dialogueSelect.SelectedDialogue.DialogueParts = new DialoguePart[DialogueList[i].DialogueParts.Length]; ;

            for (int j = 0; j < dialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                dialogueSelect.SelectedDialogue.DialogueParts[j] = new()
                {
                    EmotionSprite = DialogueList[i].DialogueParts[j].EmotionSprite,
                    PersonNameWhichTalks = DialogueList[i].DialogueParts[j].PersonNameWhichTalks,
                    SentenceThePersonTalk = DialogueList[i].DialogueParts[j].SentenceThePersonTalk
                };
            }
            tmpDialogueSelects.Add(dialogueSelect);
        }

        /*
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
        */


        var tmpDialogueSelectsDict = new Dictionary<string, DialogueSelect>();
        foreach (var tmpDialogueSelect in tmpDialogueSelects)
        {
            if (tmpDialogueSelect.SelectedDialogue.DialogueClassification != null)
            {
                tmpDialogueSelectsDict[tmpDialogueSelect.SelectedDialogue.DialogueClassification] = tmpDialogueSelect;
            }
        }

        for (int i = 0; i < dialogueClientsList.Count; i++)
        {
            var dialogueClient = dialogueClientsList[i];
            for (int j = dialogueClient.DialogueSelect.Count - 1; j >= 0; j--)
            {
                var dialogueSelect = dialogueClient.DialogueSelect[j];
                if (dialogueSelect.SelectedDialogue.DialogueClassification != null && tmpDialogueSelectsDict.TryGetValue(dialogueSelect.SelectedDialogue.DialogueClassification, out var tmpDialogueSelect))
                {
                    if (dialogueSelect.DialogueClassification != tmpDialogueSelect.DialogueClassification)
                    {
                        dialogueSelect.DialogueClassification = tmpDialogueSelect.DialogueClassification;
                    }
                }
                else
                {
                    dialogueClient.DialogueSelect.RemoveAt(j);
                }
            }

            foreach (var tmpDialogueSelect in tmpDialogueSelects)
            {
                if (!dialogueClient.DialogueSelect.Any(ds => CompareDialogueSelects(ds, tmpDialogueSelect)))
                {
                    DialogueSelect newDialogueSelect = new()
                    {
                        SelectedDialogue = tmpDialogueSelect.SelectedDialogue,
                        DialogueClassification = tmpDialogueSelect.DialogueClassification
                    };
                    dialogueClient.DialogueSelect.Add(newDialogueSelect);
                }
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

[CustomEditor(typeof(DialogueOrganizer))]
class DialogueOrganizerEditor : Editor
{
    DialogueOrganizer tmpDialogueOrganizer;
    ItemInteractionBrain tmpItemInteractionBrain;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Update all DialogueOptions"))
        {
            tmpDialogueOrganizer = FindObjectOfType<DialogueOrganizer>();
            tmpItemInteractionBrain = FindObjectOfType<ItemInteractionBrain>();

            tmpDialogueOrganizer.SyncDialoguesInDialogueClients();
            tmpItemInteractionBrain.SetDialogueSelect();
        }
    }
}

[Serializable]
public class DialogueClient : ITranslate
{
    public GameObject GOReference;
    public List<DialogueSelect> DialogueSelect = new();

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
