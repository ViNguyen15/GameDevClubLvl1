using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class DialogueManager : MonoBehaviour {

    //Queue for dialogue management using FIFO type string
    private Queue<string> sentences;

    //initialization
    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //log test on dash prefab
        Debug.Log("Starting conversation with abstract entity" + dialogue.name);

        sentences.Clear();

        //checking each array in Queue
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        //Displaying next sentence
        DisplayNextSentence();
    }

    //checking sentences in queue
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
    }

    void EndDialogue()
    {
        Debug.Log("End of dialogue");
    }

}
