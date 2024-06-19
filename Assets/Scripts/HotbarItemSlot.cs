using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarItemSlot : ItemSlot, IPointerClickHandler
{
    public static ItemSlot selectedHotbarSlot;
    public Inventory inventoryMirror;

    public void Update()
    {
        this.item = inventoryMirror.itemSlots[id].GetComponent<ItemSlot>().item;
        this.itemIconSprite.sprite = inventoryMirror.itemSlots[id].GetComponent<ItemSlot>().itemIconSprite.sprite;
    }

    // -- Sticky item cleanup moved to the coroutine
    public new void OnPointerClick(PointerEventData eventData)
    {
        selectedHotbarSlot.itemSlotSprite.sprite = selectedHotbarSlot.itemSlotDefaultSprite;
        itemSlotSprite.sprite = itemSlotSelectedSprite;

        selectedHotbarSlot = this;
    }
}