using System;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagItem;
    [SerializeField] private string tagWithItemInteractableObject;
    [SerializeField] private string tagTalkableNPCs;

    private Inventory inventory = new();

    public void Interaction(GameObject GO)
    {
        if (GO.CompareTag(tagWithItemInteractableObject) && inventory.Item != null)
        {
            print("InteractWithObject");
        }
        if (GO.CompareTag(tagTalkableNPCs))
        {
            print("TalkToNPC");
        }
        if (GO.CompareTag(tagItem))
        {
            print("ItemDetected");
        }
    }

    private void Start()
    {
        GameObject[] tmpTTNGOs = GameObject.FindGameObjectsWithTag(tagTalkableNPCs);
        GameObject[] tmpTWIIOGOs = GameObject.FindGameObjectsWithTag(tagWithItemInteractableObject);
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
    }
}

[Serializable]
public class Inventory
{
    public GameObject Item;
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