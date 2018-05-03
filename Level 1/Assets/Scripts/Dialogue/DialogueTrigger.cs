using System.Collections;
using System.Collections.Generic;

using UnityEngine;

//to show up in inspector
[System.Serializable]
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        //implement singleton later for line 15
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}