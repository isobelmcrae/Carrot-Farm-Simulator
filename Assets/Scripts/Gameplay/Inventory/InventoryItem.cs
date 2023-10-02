using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TMP_Text countText;
    [HideInInspector] public Item item; 
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    // initialises the item by setting the image and updating the count
    public void InitialiseItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
        UpdateCount();
    }

    // updates the count text and sets it to active if the count is greater than 1
    public void UpdateCount() {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // drag and drop inventory items:
    // when the player begins dragging the item, set the parent after drag to the root and set the image to not raycast
    public void OnBeginDrag(PointerEventData eventData) {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    // while the player is dragging the item, set the position to the mouse position
    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    // when the player stops dragging the item, snap the item to its new parent and set the image to raycast
    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }
}
