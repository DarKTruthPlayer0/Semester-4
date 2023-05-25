using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    private static TMP_Text dialogueName;
    private static TMP_Text dialogueText;
    private static int i = 0;
    private static int dialogueID;
    private static GameObject dialogueWindowGO;

    public static void EnterDialogue(int DialogueID)
    {
        dialogueID = DialogueID;
        //activate Dialogue Object
        dialogueName.text = DialogueOrganizer.DialoguesStatic[dialogueID].DialogueParts[i].PersonNameWhichTalks;
        dialogueText.text = DialogueOrganizer.DialoguesStatic[dialogueID].DialogueParts[i].SentenceThePersonTalk;
        i = 1;
        dialogueWindowGO.SetActive(true);
    }

    public void NextPartOfDialogue()
    {
        print("next DialoguePart");
        if (i < DialogueOrganizer.DialoguesStatic[dialogueID].DialogueParts.Length)
        {
            dialogueName.text = DialogueOrganizer.DialoguesStatic[dialogueID].DialogueParts[i].PersonNameWhichTalks;
            dialogueText.text = DialogueOrganizer.DialoguesStatic[dialogueID].DialogueParts[i].SentenceThePersonTalk;
            i++;
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        i = 0;
        dialogueName.text = null;
        dialogueText.text = null;
        //deactivate Dialogue Object
        dialogueWindowGO.SetActive(false);
    }

    private void Start()
    {
        dialogueWindowGO = GameObject.Find("DialogueWindow");
        dialogueName = dialogueWindowGO.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        dialogueText = dialogueWindowGO.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        dialogueWindowGO.SetActive(false);

    }
}
