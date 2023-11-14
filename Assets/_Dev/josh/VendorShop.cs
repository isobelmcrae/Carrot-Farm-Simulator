using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class VendorShop : MonoBehaviour
{

    public InventoryManager inventory;
    public Item Item;
    public int cost;
    public GameManager game;
    public TMP_Text YourCoins;


    public void Start()
    {
        YourCoins.text = "Coins: " + game.money.ToString();
    }

    public void BuyItem()
    {

        if (game.money >= cost)
        {
            game.RemoveMoney(cost);
            inventory.AddItem(Item);

            YourCoins.text = "Coins: " + game.money.ToString();
        }

    }

    

}
