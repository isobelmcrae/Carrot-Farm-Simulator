using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;

    void Awake() {
        if (SceneManager.GetActiveScene().name == "FarmScene") {
            dialogueText.text = "Try picking up the carrot seeds and the hoe!";
        } else {
            dialogueText.text = "insert text here";
        }
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
