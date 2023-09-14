using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveBetweenScenes : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
}
