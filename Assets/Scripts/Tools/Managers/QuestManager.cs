using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public GameObject[] questButtons;
    public TMP_Text[] buttonText;
    public List<Quest> quests = new List<Quest>();
    
    // Start is called before the first frame update
    void Start()
    {
        // loads quests into the quest menu
        for (int i = 0; i < questButtons.Length - 1; i++)
        {
            int index = Random.Range(0, quests.Count - 1);
            buttonText[i].text = quests[index].title;

            questButtons[i].GetComponent<QuestButton>().questID = quests[index].id;
            questButtons[i].GetComponent<QuestButton>().title = quests[index].title;
            questButtons[i].GetComponent<QuestButton>().description = quests[index].description;
            questButtons[i].GetComponent<QuestButton>().requirement = quests[index].requirement;
            questButtons[i].GetComponent<QuestButton>().quantity = quests[index].quantity;
            questButtons[i].GetComponent<QuestButton>().reward = quests[index].reward;

            quests.RemoveAt(index);

        }
    }
}
