using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class VendorShop : MonoBehaviour
{

    public InventoryManager inventory;
    public Item Item;
    public int cost;
    public GameManager GameManager;
    public TMP_Text YourCoins;


    public void Start()
    {
        YourCoins.text = "Coins: " + GameManager.money.ToString();
    }


    public void BuyItem()
    {

        if (GameManager.money > cost)
        {
            GameManager.RemoveMoney(cost);
            inventory.AddItem(Item);

            YourCoins.text = "Coins" + GameManager.money.ToString();
        }
          

    }
       


}
