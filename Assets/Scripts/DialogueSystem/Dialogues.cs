using System;
using System.Collections.Generic;
using UnityEngine;

public class Dialogues : MonoBehaviour
{
    public List<Dialogue> DialogueList;
}

[Serializable]
public class Dialogue
{
    public int DialogueID;
    public string DialoguePartnerName;
    public List<string> Sentences = new();
}
