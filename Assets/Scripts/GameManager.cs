using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public TileManager tileManager;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }

        // ensures that this object is not destroyed when loading new scenes
        DontDestroyOnLoad(this.gameObject);

        // gets the ItemManager and TileManager component from this object
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
    }

}
