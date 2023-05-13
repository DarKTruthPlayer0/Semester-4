using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathChoose : MonoBehaviour
{
    public GameObject PathChooseGO;
    public int InteractionID;

    [SerializeField] private Button[] decisionButtons;
    private GameObject tempPathChooseButtonsGO;
    private bool helperBool;

    public void SetButtons()
    {
        List<GameBrainScript.Style> tmpStyles = new();
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic[InteractionID].Paths.Length; i++)
        {
            tmpStyles.Add(ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].Style);
        }
        string[] tempStyleNames = Enum.GetNames(typeof(GameBrainScript.Style)); 

        for (int i = 0; i < tempPathChooseButtonsGO.transform.childCount; i++)
        {
            tempPathChooseButtonsGO.transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = tempStyleNames[i];
            for (int j = 0; j < tmpStyles.Count; j++)
            {
                if (tmpStyles[j].ToString() == tempStyleNames[i])
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
            tempPathChooseButtonsGO.transform.GetChild(i).gameObject.SetActive(false);
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
                StartInteractionEvent((GameBrainScript.Style)i);
                break;
            }

        }
    }

    private void StartInteractionEvent(GameBrainScript.Style style)
    {
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic[InteractionID].Paths.Length; i++)
        {
            if (ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].Style == style)
            {
                //Start Dialogue
                DialogueSystem.EnterDialogue(ItemInteractionBrain.InteractionsStatic[InteractionID].Paths[i].InteractionDialogueID);
            }
        }
    }

    private void Awake()
    {
        tempPathChooseButtonsGO = GameObject.Find("PathChooseButtons");
    }
}
