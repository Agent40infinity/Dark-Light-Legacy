using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public float[] interval = new float[2];
    public int intervalIndex;
    public List<string> dialogue = new List<string>();
    public int index;
    public string display;
    public DialogueState dialogueState = DialogueState.Idle;

    public Text dialogueText;

    public void Update()
    {
        if (Input.GetKeyDown(GameManager.keybind["Interact"]) || Input.GetKeyDown(GameManager.keybind["Jump"]) || Input.GetMouseButtonDown(0))
        {
            switch (dialogueState)
            {
                case DialogueState.Idle:
                    StartCoroutine(DisplayText(dialogue[index]));
                    break;
                case DialogueState.Normal:
                    intervalIndex = 1;
                    dialogueState = DialogueState.Fast;
                    break;
            }
        }
    }

    public void UpdateText()
    {
        dialogueText.text = display;
    }

    public IEnumerator DisplayText(string sentence)
    {
        for (int i = 0; i < sentence.Length; i++)
        {
            display = sentence.Substring(0, i);
            UpdateText();
            yield return new WaitForSeconds(interval[intervalIndex]);
        }

        index++;
        intervalIndex = 0;
        dialogueState = DialogueState.Idle;
    }
}

public enum DialogueState
{
    Idle,
    Normal,
    Fast
}