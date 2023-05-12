using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagItem;
    [SerializeField] private string tagInteractable;
    [SerializeField] private string tagTalkableNPCs;
    [SerializeField] private string tagInventoryPlaces;

    private Inventory inventory = new();
    private PathChoose pathChoose;
    private GameObject pathChooseButtonsGO;
    private int selectedItemID;

    public void Interaction(GameObject IGO)
    {
        if (IGO.CompareTag(tagInteractable))
        {
            print("Interactable");
            if (inventory.Items[selectedItemID].ItemSelected)
            {
                UseItemInIventory(IGO);
            }
            else
            {
                for (int i = 0; i < DialogueOrganizer.DialogueClientsStatic.Length; i++)
                {
                    if (DialogueOrganizer.DialogueClientsStatic[i].GOReference == IGO)
                    {
                        DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[i].dialogueID);
                        break;
                    }

                }
            }
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

    public void SelectItemInInventory(Button Button)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (Button == inventory.Items[i].InventoryPlace && !inventory.Items[i].ItemSelected && inventory.Items[i].ItemGO != null)
            {
                inventory.Items[i].ItemSelected = true;
                selectedItemID = i;

                for (int j = 0; j < inventory.Items.Length; j++)
                {
                    if (j == selectedItemID)
                    {
                        continue;
                    }
                    inventory.Items[j].ItemSelected = false;
                }
                break;
            }
            else if (Button == inventory.Items[i].InventoryPlace && inventory.Items[i].ItemSelected && inventory.Items[i].ItemGO != null)
            {
                inventory.Items[i].ItemSelected = false;
                break;
            }
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
    private void PickupItem(GameObject TempItemGO)
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            if (inventory.Items[i].ItemGO == null)
            {
                print("HI");
                inventory.Items[i].ItemGO = TempItemGO;
                //Dialog einblenden
                for (int j = 0; j < DialogueOrganizer.DialogueClientsStatic.Length; j++)
                {
                    if (TempItemGO == DialogueOrganizer.DialogueClientsStatic[j].GOReference)
                    {
                        DialogueSystem.EnterDialogue(DialogueOrganizer.DialogueClientsStatic[j].dialogueID);
                        break;
                    }
                }
                TempItemGO.SetActive(false);
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

    private void SetUpIventory()
    {
        GameObject[] tempButtonGOs = GameObject.FindGameObjectsWithTag(tagInventoryPlaces);

        inventory.Items = new Item[tempButtonGOs.Length];
        for (int j = 0;j < tempButtonGOs.Length; j++)
        {
            inventory.Items[j] = new Item
            {
                InventoryPlace = tempButtonGOs[j].GetComponent<Button>()
            };
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

        SetUpIventory();
    }
}

[Serializable]
public class Inventory
{
    public Item[] Items;
}
[Serializable]
public class Item
{
    public GameObject ItemGO;
    public Button InventoryPlace;
    public bool ItemSelected;
}
public class PlayerServant : MonoBehaviour
{
    private PlayerBrain pBrain;

    private void OnMouseDown()
    {
        print("Mouse down on" + gameObject.name);
        pBrain.Interaction(gameObject);
    }

    private void Start()
    {
        pBrain = FindObjectOfType<PlayerBrain>();
    }
}