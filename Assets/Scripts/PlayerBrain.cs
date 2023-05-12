using System;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagItem;
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagTalkableNPCs;

    private Inventory inventory = new();
    private PathChoose pathChoose;
    private GameObject pathChooseButtonsGO;
    private int selectedItemID;
    private bool itemIsSelected;

    public void Interaction(GameObject IGO)
    {
        if (IGO.CompareTag(tagInteractable) /*&& inventory.Item != null*/ && itemIsSelected)
        {
            print("Interactable");
            UseItemInIventory(IGO);
        }
        if (IGO.CompareTag(tagTalkableNPCs))
        {
            print("TalkToNPC");
        }
        if (IGO.CompareTag(tagItem))
        {
            print("ItemDetected");
            PickupItem(IGO);
        }
    }

    private void UseItemInIventory(GameObject UIIIGO)
    {
        for (int i = 0; i < ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic.Length; i++)
        {
            if (ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.Item != inventory.Items[selectedItemID].ItemGO)
            {
                continue;
            }
            for (int j = 0; j < ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.InteractionObjects.Count; j++)
            {
                if (ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.InteractionObjects[j].InteractWithObject &&
                    ItemInteractionAssingmentLoad.ItemToObjectsAssingmentsStatic[i].ItemMatchingInteractionObject.InteractionObjects[j].Object == UIIIGO)
                {
                    CheckInteractionOutcome(UIIIGO);
                    break;

                }
            }
        }
    }

    private void CheckInteractionOutcome(GameObject CIOGO)
    {
        for (int i = 0; i < ItemInteractionBrain.InteractionsStatic.Length; i++)
        {
            if (ItemInteractionBrain.InteractionsStatic[i].Interactable == CIOGO &&
                ItemInteractionBrain.InteractionsStatic[i].Item == inventory.Items[selectedItemID].ItemGO)
            {
                //Path Choose
                pathChoose.InteractionID = i;
                pathChoose.SetButtons();
            }
        }
    }
    private void PickupItem(GameObject ItemGO)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i] == null)
            {
                ItemGO.SetActive(false);
                inventory.Items[i].ItemGO = ItemGO;
                //Dialog einblenden
                for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic.Length; j++)
                {
                    if (ItemGO == DialogueOrganizer.DialogueClientsStatic[j].GOReference)
                    {
                        DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[j].dialogueID);
                        break;
                    }
                }
                break;
            }
        }
    }

    private void TalkWithNPC(GameObject TWNPCGO)
    {
        for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic.Length; j++)
        {
            if (TWNPCGO == DialogueOrganizer.DialogueClientsStatic[j].GOReference)
            {
                DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[j].dialogueID);
                break;
            }
        }
    }

    private void Start()
    {
        GameObject[] tmpTTNGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tmpTWIIOGOs = GameObject.FindGameObjectsWithTag(tagInteractable);
        GameObject[] tmpTIGOs = GameObject.FindGameObjectsWithTag(tagItem);

        foreach (GameObject go in tmpTTNGOs)
        {
            go.AddComponent<PlayerServant>();
        }
        foreach (GameObject go in tmpTWIIOGOs)
        { 
            go.AddComponent<PlayerServant>();
        }
        foreach(GameObject go in tmpTIGOs)
        { 
            go.AddComponent<PlayerServant>();
        }

        pathChoose = FindObjectOfType<PathChoose>();
        pathChooseButtonsGO = GameObject.Find("PathChooseButtons");
        pathChooseButtonsGO.SetActive(false);
    }
}

[Serializable]
public class Inventory
{
    public Item[] Items = new Item[3];
}
[Serializable]
public class Item
{
    public GameObject ItemGO;
    public bool ItemSelected;
}
public class PlayerServant : MonoBehaviour
{
    private PlayerBrain pBrain;

    private void OnMouseDown()
    {
        pBrain.Interaction(gameObject);
    }

    private void Start()
    {
        pBrain = FindObjectOfType<PlayerBrain>();
    }
}