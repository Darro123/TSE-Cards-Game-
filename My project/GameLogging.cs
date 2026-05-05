using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameLogUI : MonoBehaviour
{
    public static GameLogUI Instance;

    public TMP_Text logText;

    private Queue<string> messages = new Queue<string>();

    public int maxMessages = 8;

    private void Awake()
    {
        Instance = this;
    }

    public void AddMessage(string message)
    {
        messages.Enqueue(message);

        while (messages.Count > maxMessages)
        {
            messages.Dequeue();
        }

        logText.text = "";

        foreach (string msg in messages)
        {
            logText.text += msg + "\n";
        }

        Debug.Log(message);
    }
}
