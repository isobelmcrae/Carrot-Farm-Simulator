using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Quest")]
public class Quest : ScriptableObject
{
    public string title;
    public string description;

    public Item requirement;
    public int quantity;
    public int reward;

    public int id;
    
}
