using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IAcceptItem, IPointerClickHandler
{
    public static ItemSlot selectedItemSlot;
    public int id;
    public bool hotbar;
    public Inventory inventoryMirror;   //optional, only needed for hotbar since it mirrors inventory
    public Item item;
    public GameObject itemIcon;
    public Image spriteImage;
    public Sprite transparentSprite;

    public void Update()
    {
        if (hotbar)
        {
            this.item = inventoryMirror.itemSlots[id].GetComponent<ItemSlot>().item;
        }
    }

    public Item AcceptItem(Item item, ItemSlot itemSlot)
    {
        Item returned = this.item;

        this.item = item;
        if (item != null)
        {
            this.spriteImage.sprite = item.sprite;
            this.item.gameObject.transform.SetParent(this.transform);
        }
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

            // -- Hotbar has no dragging
            if (!hotbar) StartCoroutine(StickItemIconToCursor(itemIcon));

            // -- Up the sort order by one, so it renders over other items you hover over
            itemIcon.GetComponent<Canvas>().sortingOrder++;
        }
        // -- If clicked on another item slot, transfer items. (This method only triggers on item slots, bcs of IPointerClickHandler)
        else
        {
            StickyItemCleanup();
        }
    }

    // -- You can call this on inventory close, since otherwise - what would you do with a sticky item
    public void StickyItemCleanup()
    {
        // -- Transfer items
        Item incomingItem = selectedItemSlot.item;
        selectedItemSlot.AcceptItem(AcceptItem(incomingItem, selectedItemSlot), this);

        // -- Put icons back where they belong
        itemIcon.transform.localPosition = Vector3.zero;
        selectedItemSlot.itemIcon.transform.localPosition = Vector3.zero;

        // -- Lower the sort order by one, since you upped it on click (this time on the selectedItemSlot, since "perspective" changed)
        selectedItemSlot.itemIcon.GetComponent<Canvas>().sortingOrder--;

        selectedItemSlot = null;
    }

    public IEnumerator StickItemIconToCursor(GameObject itemIcon)
    {
        if (itemIcon == null) { yield return null; }

        while (selectedItemSlot != null)
        {
            itemIcon.transform.position = Input.mousePosition;
            yield return null;
        }
    }
}