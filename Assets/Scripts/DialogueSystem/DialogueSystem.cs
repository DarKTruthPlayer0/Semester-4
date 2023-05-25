using UnityEngine;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    private static TMP_Text dialogueName;
    private static TMP_Text dialogueText;
    private static int i = 0;
    private static Dialogue tmpDialogue;
    private static GameObject dialogueWindowGO;

    public static void EnterDialogue(Dialogue dialogue)
    {
        tmpDialogue = dialogue;
        //activate Dialogue Object
        dialogueName.text = tmpDialogue.DialogueParts[i].PersonNameWhichTalks;
        dialogueText.text = tmpDialogue.DialogueParts[i].SentenceThePersonTalk;
        i = 1;
        dialogueWindowGO.SetActive(true);
    }

    public void NextPartOfDialogue()
    {
        print("next DialoguePart");
        if (i < tmpDialogue.DialogueParts.Length)
        {
            dialogueName.text = tmpDialogue.DialogueParts[i].PersonNameWhichTalks;
            dialogueText.text = tmpDialogue.DialogueParts[i].SentenceThePersonTalk;
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
        tmpDialogue = null;
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
