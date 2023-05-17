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
    [SerializeField] Dialogues dialogues;
    [SerializeField] private List<DialogueClient> dialogueClientsList;

    private void Start()
    {
        DialogueClientsStatic = dialogueClientsList.ToArray();
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            AssingDialogueClients();
            SetDialogueIDs();
        }
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
    }

    private void SetDialogueIDs()
    {
        for (int i = 0; i < dialogues.DialogueList.Count; i++)
        {
            dialogues.DialogueList[i].DialogueID = i;
        }
    }
}

[Serializable]
public class DialogueClient : Translate
{
    public GameObject GOReference;
    public int dialogueID;

    public GameObject GOTranslate
    {
        get { return GOReference; }
        set { GOReference = value; }
    }
}
