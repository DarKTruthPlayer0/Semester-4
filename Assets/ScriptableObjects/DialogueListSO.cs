using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DialogueList")]
public class DialogueListSO : ScriptableObject
{
    public List<Dialogue> dialogueList;
}
