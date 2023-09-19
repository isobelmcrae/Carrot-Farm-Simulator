using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NpcDialogue : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;
    public float wordSpeed;
    public bool playerIsClose;
    public GameObject contButton;

    private bool typing = false;
    private bool skipped = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                StopAllCoroutines();
                dialogueText.text = ""; 
                dialoguePanel.SetActive(false);
                index = 0;


            }
            else
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }

        }

        //if (dialogueText.text == dialogue[index])
        // {
        //   contButton.SetActive(true);
        //}

        if (Input.GetKeyDown(KeyCode.Space) && dialoguePanel.activeInHierarchy)
        {
            if(!typing)
            {
                NextLine();
            }
            else
            {
                skipped = true;
                //dialogueText.text = dialogue[index];
            }
            
        }

    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        typing = true;

        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed * (skipped ? 0.01f : 1f));

        }

        typing = false;
        skipped = false;
    }

    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing()); 
        }
        else
        {
            zeroText();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;

            dialogueText.text = "";
        }

        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
        zeroText();

        StopAllCoroutines();
    }
}
