using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathChoose : MonoBehaviour
{
    public static GameObject PathChooseGO;
    public int InteractionID;

    [SerializeField] private Button[] decisionButtons;

    public void SetButtons()
    {
        for (int i = 0; i < decisionButtons.Length; i++)
        {
            decisionButtons[i].transform.GetChild(0).GetComponent<TMP_Text>().text = Enum.GetName(typeof(GameBrainScript.Style), i);
        }
    }

    public void DecisionTaken(GameObject GO)
    {
        string[] tempStylenames = Enum.GetNames(typeof(GameBrainScript.Style));
        for (int i = 0; i < tempStylenames.Length; i++)
        {
            if (GO.transform.GetChild(0).GetComponent<TMP_Text>().text == Enum.GetName(typeof(GameBrainScript.Style), i))
            {
                GameBrainScript.styles.Add((GameBrainScript.Style)i);

                break;
            }

        }
    }

    private void StartInteractionEvent(GameBrainScript.Style style)
    {
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic.Length; i++)
        {
            if (ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].Style == style)
            {
                //Start Dialogue
                DialogueSystem.EnterDialogue(ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].InteractionDialogueID);
            }
        }
    }
}
