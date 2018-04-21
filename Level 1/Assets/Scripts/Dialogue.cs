using System.Collections;
using System.Collections.Generic;

using UnityEngine;

//to show up in inspector
[System.Serializable]
public class Dialogue
{
    //npc name or object?
    public string name;

    //TextArea to limit/manage text min to max amount of sentences
    [TextArea(3,10)]
    public string[] sentences;

}