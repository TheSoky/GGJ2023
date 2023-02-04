using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeManager : MonoBehaviour
{
    public Queue<string> sentences;
    void Start()
    {
        sentences = new Queue<string>();
    }
    public void StartDialog(Dialog dial) //Starts the Dialog
    {
        Debug.Log("Chatting with: " + dial.name);
        sentences.Clear();
        foreach(string sentence in dial.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNext();
    }
    public void DisplayNext() //Button "continue" triggers this until its finished with dialog
    {
        if(sentences.Count == 0)
        {
            Debug.Log("Chat ended");//ends the Dialog
            return;
        }
        string sentence = sentences.Dequeue();
        Debug.Log(sentence);        //still adding TEXT panell
    }
}
