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
                        Style = DialogueListSO.dialogueList[i].DialogueParts[j].Emotions[k].Style
                    };
                }
            }
            tmpDialogueSelects.Add(dialogueSelect);
        }

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
                if (!dialogueClient.DialogueSelect.Any(dialogueSelect => CompareDialogueSelects(dialogueSelect, tmpDialogueSelect)))
                {
                    DialogueSelect newDialogueSelect = new()
                    {
                        SelectedDialogue = tmpDialogueSelect.SelectedDialogue,
                        DialogueClassification = tmpDialogueSelect.DialogueClassification
                    };
                    dialogueClient.DialogueSelect.Add(newDialogueSelect);
                }
            }

            // Synchronisiere die Emotions-Arrays
            for (int j = 0; j < dialogueClient.DialogueSelect.Count; j++)
            {
                var compareDialogueSelect = dialogueClient.DialogueSelect[j].SelectedDialogue;
                for (int k = 0; k < tmpDialogueSelects.Count; k++)
                {
                    var tmpDialogueSelect = tmpDialogueSelects[k].SelectedDialogue;
                    for (int l = 0; l < compareDialogueSelect.DialogueParts.Length; l++)
                    {
                        if (tmpDialogueSelect.DialogueClassification != compareDialogueSelect.DialogueClassification)
                        {
                            continue;
                        }
                        compareDialogueSelect.DialogueParts[l].Emotions = new Emotion[tmpDialogueSelect.DialogueParts[l].Emotions.Length];
                        for (int m = 0; m < compareDialogueSelect.DialogueParts[l].Emotions.Length; m++)
                        {
                            compareDialogueSelect.DialogueParts[l].Emotions[m] = new();
                            if (tmpDialogueSelect.DialogueParts[l].Emotions[m].EmotionSprite != null)
                            {
                                compareDialogueSelect.DialogueParts[l].Emotions[m].EmotionSprite = tmpDialogueSelect.DialogueParts[l].Emotions[m].EmotionSprite;
                            }
                            if (tmpDialogueSelect.DialogueParts[l].Emotions[m].EmotionSound != null)
                            {
                                compareDialogueSelect.DialogueParts[l].Emotions[m].EmotionSound = tmpDialogueSelect.DialogueParts[l].Emotions[m].EmotionSound;
                            }
                            compareDialogueSelect.DialogueParts[l].Emotions[m].Style = tmpDialogueSelect.DialogueParts[l].Emotions[m].Style;
                        }
                    }
                }
            }
        }
    }
 
    private bool CompareDialogueSelects(DialogueSelect DialogueClientsDialogueSelect, DialogueSelect tmpDialogueSlectsDialogueSelect)
    {
        count = 0;

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

        if (count != DialogueClientsDialogueSelect.SelectedDialogue.DialogueParts.Length)
        {
            return false;
        }

        //Angleiches der DialogueClassification
        if (DialogueClientsDialogueSelect.DialogueClassification != tmpDialogueSlectsDialogueSelect.DialogueClassification)
        {
            DialogueClientsDialogueSelect.DialogueClassification = tmpDialogueSlectsDialogueSelect.DialogueClassification;
            DialogueClientsDialogueSelect.SelectedDialogue.DialogueClassification = tmpDialogueSlectsDialogueSelect.SelectedDialogue.DialogueClassification;
        }
        return true;
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
    public Emotion[] Emotions;
    public string PersonNameWhichTalks;
    public string SentenceThePersonTalk;
}

[Serializable]
public class Emotion
{
    public Sprite EmotionSprite;
    public AudioClip EmotionSound;
    public GameBrainScript.Style Style;
}
