using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickButton : MonoBehaviour
{
    public int reward;
    public Item requirement;
    public InventoryManager inventoryManager;
    public GameManager game;

    public void Redeem() {
        if (inventoryManager.GetSelectedItem(false) == requirement) {
            inventoryManager.GetSelectedItem(true);
            game.AddMoney(reward);
        }
    }
}
