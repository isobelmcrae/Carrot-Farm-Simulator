using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    public int questID;
    public string title;
    public string description;
    public Item requirement;
    public int quantity;
    public int reward;

    public GameObject questPopout;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Image itemImage;
    public TMP_Text quantityText;
    public TMP_Text rewardText;

    public void ToggleState() {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void ShowPopout() {
        titleText.text = title;
        descriptionText.text = description;
        itemImage.sprite = requirement.image;
        quantityText.text = quantity.ToString();
        rewardText.text = reward.ToString();
        questPopout.SetActive(true);
    }
}
