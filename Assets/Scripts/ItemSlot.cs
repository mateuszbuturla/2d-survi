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

    // -- Sticky item cleanup moved to the coroutine
    public void OnPointerClick(PointerEventData eventData)
    {
        // -- Pick up item
        if (selectedItemSlot == null)
        {
            if (item == null) return;

            selectedItemSlot = this;

            StartCoroutine(StickItemIconToCursor(itemIcon));
        }
        // -- If clicked on another item slot, transfer items. (This method only triggers on item slots, bcs of IPointerClickHandler)
        else
        {
            // -- Transfer items
            Item incomingItem = selectedItemSlot.item;
            selectedItemSlot.AcceptItem(AcceptItem(incomingItem, selectedItemSlot), this);
        }
    }

    public IEnumerator StickItemIconToCursor(GameObject itemIcon)
    {
        if (itemIcon == null) { yield return null; }

        // -- Up the sort order by one, so it renders over other items you hover over
        itemIcon.GetComponent<Canvas>().sortingOrder++;

        while (selectedItemSlot != null)
        {
            itemIcon.transform.position = Input.mousePosition;
            yield return null;
        }

        // -- Put icons back where they belong
        itemIcon.transform.localPosition = Vector3.zero;
        selectedItemSlot.itemIcon.transform.localPosition = Vector3.zero;

        // -- Lower the sort order by one, since you upped it on click (this time on the selectedItemSlot, since "perspective" changed)
        selectedItemSlot.itemIcon.GetComponent<Canvas>().sortingOrder--;

        // -- Has to be here to avoid nullref during cleanup
        selectedItemSlot = null;
    }
}