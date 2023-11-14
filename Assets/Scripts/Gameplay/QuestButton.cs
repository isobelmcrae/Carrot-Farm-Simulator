using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void toggleState() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
