using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TMP_Text[] questButtons;
    public List<Quest> quests = new List<Quest>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < questButtons.Length; i++)
        {
            int index = Random.Range(0, quests.Count - 1);
            questButtons[i].text = quests[index].title;
            quests.RemoveAt(index);

        }
    }
}
