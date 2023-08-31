using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;

    void Awake() {
        dialogueText.text = "Try picking up the carrot seeds and the hoe!";
    }

    public void SetDialogue(string dialogue) {
        dialogueText.text = dialogue;
    }

    public void tutorialText(int id) {
        if (id == 1) {
            SetDialogue("Use the number keys to select an item in your inventory, and left click to use it!");
        } else return;
    }
}
