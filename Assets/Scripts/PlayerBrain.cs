using System;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    [Header("Tags")]
    [SerializeField] private string tagWithItemInteractableObject;
    [SerializeField] private string tagTalkableNPCs;

    [Header("InteractionSettings")]
    [SerializeField] private LayerMask interactionLayerMask;
    private RaycastHit hit;
    private Camera cam;
    private Vector2 mPosition = Vector2.zero;
    private Inventory inventory = new();

    private void PlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mPosition = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(mPosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayerMask))
            {
                Interaction();
            }
        }
    }

    private void Interaction()
    {
        if (hit.transform.gameObject.CompareTag(tagWithItemInteractableObject) && inventory.Item != null)
        {
            print("InteractWithObject");
        }
        if (hit.transform.gameObject.CompareTag(tagTalkableNPCs))
        {
            print("TalkToNPC");
        }
    }

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        PlayerClick();
    }
}

[Serializable]
public class Inventory
{
    public GameObject Item;
}
