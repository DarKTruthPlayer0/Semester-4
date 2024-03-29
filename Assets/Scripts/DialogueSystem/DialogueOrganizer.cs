using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DialogueOrganizer : ListFunctionsExtension
{
    [Header("Tags")]
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagItems;
    [SerializeField] private string tagInteractables;

    public DialogueListSO DialogueListSO;

    public static DialogueClient[] DialogueClientsStatic;
    public static Dialogue[] DialoguesStatic;
    [SerializeField] private List<DialogueClient> dialogueClientsList;

    private bool helperBool3;

    private bool sentenceCheckBool;
    private bool personNameCheckBool;
    private int count;

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
    }

    public void SyncDialoguesInDialogueClients()
    {
        List<DialogueSelect> tmpDialogueSelects = new();

        for (int i = 0; i < DialogueListSO.dialogueList.Count; i++)
        {

            DialogueSelect dialogueSelect = new()
            {
                DialogueClassification = DialogueListSO.dialogueList[i].DialogueClassification
            };
            dialogueSelect.SelectedDialogue.DialogueClassification = dialogueSelect.DialogueClassification;
            dialogueSelect.SelectedDialogue.DialogueParts = new DialoguePart[DialogueListSO.dialogueList[i].DialogueParts.Length]; ;

            for (int j = 0; j < dialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                dialogueSelect.SelectedDialogue.DialogueParts[j] = new()
                {
                    Emotions = new Emotion[DialogueListSO.dialogueList[i].DialogueParts[j].Emotions.Length],
                    PersonNameWhichTalks = DialogueListSO.dialogueList[i].DialogueParts[j].PersonNameWhichTalks,
                    SentenceThePersonTalk = DialogueListSO.dialogueList[i].DialogueParts[j].SentenceThePersonTalk
                };
                for (int k = 0; k < dialogueSelect.SelectedDialogue.DialogueParts[j].Emotions.Length; k++)
                {
                    dialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k] = new()
                    {
                        EmotionSprite = DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].EmotionSprite,
                        EmotionSound = DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].EmotionSound,
                        style = DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].style
                    };
                }
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
                    if (CompareDialogueSelects(dialogueClientsList[i].DialogueSelect[j], tmpDialogueSelects[k]))
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


        var tmpDialogueSelectsSet = new HashSet<DialogueSelect>(tmpDialogueSelects);

        for (int i = 0; i < dialogueClientsList.Count; i++)
        {
            var dialogueClient = dialogueClientsList[i];
            for (int j = dialogueClient.DialogueSelect.Count - 1; j >= 0; j--)
            {
                var dialogueSelect = dialogueClient.DialogueSelect[j];
                if (!tmpDialogueSelectsSet.Any(tmpDialogueSelect => CompareDialogueSelects(dialogueSelect, tmpDialogueSelect)))
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
        count = 0;
        helperBool3 = false;

        for (int i = 0; i < tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts.Length; i++)
        {
            for (int j = 0; j < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                sentenceCheckBool = false;
                personNameCheckBool = false;
                if (string.Equals(DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].SentenceThePersonTalk, tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].SentenceThePersonTalk))
                {
                    sentenceCheckBool = true;
                }
                if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].PersonNameWhichTalks == tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].PersonNameWhichTalks)
                {
                    personNameCheckBool = true;
                }
                if (sentenceCheckBool && personNameCheckBool)
                {
                    count++;
                }
            }
        }

        if (count == DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length)
        {
            helperBool3 = true;
        }

        //Angleiches der DialogueClassification
        if (DialogueClientsDialogueSelect.DialogueClassification != tmpDialogueSlectsDialogueSelect.DialogueClassification && helperBool3)
        {
            DialogueClientsDialogueSelect.DialogueClassification = tmpDialogueSlectsDialogueSelect.DialogueClassification;
            DialogueClientsDialogueSelect.SelectedDialogue.DialogueClassification = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueClassification;
        }

        //Angleichen der EmotionSprites
        for (int i = 0; i < tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts.Length; i++)
        {
            for (int j = 0; j < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length; j++)
            {
                for (int k = 0; k < DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].Emotions.Length; k++)
                {
                    if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k].EmotionSprite != tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].Emotions[k].EmotionSprite && helperBool3)
                    {
                        DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k].EmotionSprite = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].Emotions[k].EmotionSprite;
                    }

                    if (DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k].EmotionSound != tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].Emotions[k].EmotionSound && helperBool3)
                    {
                        DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts[j].Emotions[k].EmotionSound = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueParts[i].Emotions[k].EmotionSound;
                    }
                }
            }
        }
        return helperBool3;
    }
}

#if UNITY_EDITOR
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
#endif

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
    [HideInInspector] public Dialogue SelectedDialogue = new();
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
    public Emotion[] Emotions = new Emotion[3];
    public string PersonNameWhichTalks;
    public string SentenceThePersonTalk;
}

[Serializable]
public class Emotion
{
    public Sprite EmotionSprite;
    public AudioClip EmotionSound;
    public GameBrainScript.Style style;
}
