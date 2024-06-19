using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicInventory : Inventory
{
    public int slotsAmount;

    private void Start()
    {
        CreateItemSlots();
    }

    void OnDisable()
    {
        if (ItemSlot.selectedItemSlot != null) 
        { 
            ItemSlot.selectedItemSlot.StickyItemCleanup();
        }
    }

    private void CreateItemSlots()
    {
        for (int i = 0; i < slotsAmount; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab);
            itemSlot.transform.SetParent(transform);
            ItemSlot itemSlotComponent = itemSlot.GetComponent<ItemSlot>();
            itemSlotComponent.id = i;

            itemSlots.Add(itemSlot);
        }
    }
}
