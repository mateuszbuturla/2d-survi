using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarItemSlot : ItemSlot, IPointerClickHandler
{
    public static ItemSlot selectedHotbarSlot;
    public Inventory inventoryMirror;

    void Start()
    {
        if (selectedHotbarSlot == null) { selectedHotbarSlot = this; SetSelected(this); }
    }

    public void Update()
    {
        this.item = inventoryMirror.itemSlots[id].GetComponent<ItemSlot>().item;
        this.itemIconSprite.sprite = inventoryMirror.itemSlots[id].GetComponent<ItemSlot>().itemIconSprite.sprite;
    }

    // -- Sticky item cleanup moved to the coroutine
    public new void OnPointerClick(PointerEventData eventData)
    {
        SetSelected(this);
    }

    public static void SetSelected(HotbarItemSlot itemSlot)
    {
        selectedHotbarSlot.itemSlotSprite.sprite = selectedHotbarSlot.itemSlotDefaultSprite;

        itemSlot.itemSlotSprite.sprite = itemSlot.itemSlotSelectedSprite;
        selectedHotbarSlot = itemSlot;
    }
}