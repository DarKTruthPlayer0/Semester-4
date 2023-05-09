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
    public DialoguePart[] DialogueParts;
}

[Serializable]
public class DialoguePart
{
    public string PersonNameWhichTalks;
    public string SentenceThePersonTalk;
}
