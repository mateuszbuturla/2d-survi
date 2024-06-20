using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IAcceptItem, IPointerClickHandler
{
    public static ItemSlot selectedItemSlot;
    public int id;
    public Item item;
    public GameObject itemIcon;
    public Image itemIconSprite;
    public Sprite transparentSprite;
    public Image itemSlotSprite;
    public Sprite itemSlotDefaultSprite;
    public Sprite itemSlotSelectedSprite;

    public void DecreateItemCount(int amount)
    {
        item.amount -= amount;
        if (item.amount <= 0)
        {
            DestroyItem();
        }
    }

    public void DestroyItem()
    {
        Destroy(item.gameObject);
        item = null;
        itemIconSprite.sprite = null;
    }

    public Item AcceptItem(Item item)
    {
        Item returned = this.item;

        if (this.item != null)
        {
            if (this.item.name == item.name)
            {
                returned = StackItem(item);
            }
        }
        else
        {
            this.item = item;

            if (item != null)
            {
                this.itemIconSprite.sprite = item.sprite;
                this.item.gameObject.transform.SetParent(this.transform);
            }
            else this.itemIconSprite.sprite = transparentSprite;
        }

        return returned;
    }

    public Item AcceptItem(GameObject item)
    {
        Item returned = this.item;

        Item incomingItem = item.GetComponent<Item>();

        if (this.item.name == item.name)
        {
            returned = StackItem(incomingItem);
        }
        else
        {
            this.item = incomingItem;

            if (item != null)
            {
                this.itemIconSprite.sprite = incomingItem.sprite;
                this.item.gameObject.transform.SetParent(this.transform);
            }
            else this.itemIconSprite.sprite = transparentSprite;
        }

        return returned;
    }

    // -- Sticky item cleanup moved to the coroutine
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PickItem();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item is Openable)
            {
                (item as Openable).RightClickEffect();
            }
        }
    }

    private void PickItem()
    {
        // -- Pick up item
        if (selectedItemSlot == null)
        {
            if (item == null) return;

            selectedItemSlot = this;

            // -- Hotbar has no dragging
            StartCoroutine(StickItemIconToCursor(itemIcon));

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
        selectedItemSlot.AcceptItem(AcceptItem(incomingItem));

        // -- Put icons back where they belong
        itemIcon.transform.localPosition = Vector3.zero;
        selectedItemSlot.itemIcon.transform.localPosition = Vector3.zero;

        // -- Lower the sort order by one, since you upped it on click (this time on the selectedItemSlot, since "perspective" changed)
        selectedItemSlot.itemIcon.GetComponent<Canvas>().sortingOrder--;

        // -- Reset selected sprite to default
        selectedItemSlot.itemSlotSprite.sprite = selectedItemSlot.itemSlotDefaultSprite;

        selectedItemSlot = null;
    }

    public IEnumerator StickItemIconToCursor(GameObject itemIcon)
    {
        if (itemIcon == null) { yield return null; }

        while (selectedItemSlot != null)
        {
            selectedItemSlot.itemSlotSprite.sprite = selectedItemSlot.itemSlotSelectedSprite;
            itemIcon.transform.position = Input.mousePosition;
            yield return null;
        }
    }

    private Item StackItem(Item incomingItem)
    {
        int capacity = this.item.maxStack - this.item.amount;

        if (incomingItem.amount > capacity)
        {
            incomingItem.amount -= capacity;
            this.item.amount += capacity;

            return incomingItem;
        }
        else
        {
            this.item.amount =+ incomingItem.amount;
            return null;
        }
    }
}