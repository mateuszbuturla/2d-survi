using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IAcceptItem, IPointerClickHandler
{
    public static ItemSlot selectedItemSlot;
    public int id;
    public Item item;
    public GameObject itemIcon;
    private Vector3 itemIconStartPos;
    public Image spriteImage;
    public Sprite transparentSprite;

    public Item AcceptItem(Item item, ItemSlot itemSlot)
    {
        Item returned = this.item;

        this.item = item;
        if (item != null) this.spriteImage.sprite = item.sprite;
        else this.spriteImage.sprite = transparentSprite;

        return returned;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("click");
        if (selectedItemSlot == null)
        {
            if (item == null) return;

            selectedItemSlot = this;
            itemIcon.GetComponent<Image>().raycastTarget = false;
            StartCoroutine(StickItemIconToCursor(itemIcon));
        }
        else
        {
            itemIcon.transform.position = itemIconStartPos;
            Item incomingItem = selectedItemSlot.item;
            selectedItemSlot.AcceptItem(AcceptItem(incomingItem, selectedItemSlot), this);
            selectedItemSlot = null;
        }
    }

    public IEnumerator StickItemIconToCursor(GameObject itemIcon)
    {
        if (itemIcon == null) { yield return null; }

        itemIconStartPos = itemIcon.transform.position;
        while (selectedItemSlot != null)
        {
            itemIcon.transform.position = Input.mousePosition;
            yield return 0;
        }
        yield return null;
    }
}