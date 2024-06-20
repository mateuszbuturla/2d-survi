using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public List<GameObject> itemSlots;
    public GameObject itemSlotPrefab;

    void OnDisable()
    {
        if (ItemSlot.selectedItemSlot != null)
        {
            ItemSlot.selectedItemSlot.StickyItemCleanup();
        }
    }

    public bool AddItem(Item item)
    {
        foreach (GameObject slot in itemSlots)
        {
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
            if (itemSlot.item == null)
            {
                itemSlot.AcceptItem(item);
                return true;
            }
        }
        return false;
    }

    public bool AddItem(GameObject item)
    {
        foreach (GameObject slot in itemSlots)
        {
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
            if (itemSlot.item == null)
            {
                itemSlot.AcceptItem(item);
                return true;
            }
        }
        return false;
    }

    public void DecreateItemCount(int slotId, int amount = 1)
    {
        GameObject slot = itemSlots[slotId];
        slot.GetComponent<ItemSlot>().DecreateItemCount(amount);
    }

    public bool CheckIfContainsItem(Item item, int requiredAmount = 1)
    {
        int amountFound = 0;

        foreach (GameObject slot in itemSlots)
        {
            ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
            if (itemSlot.item != null && itemSlot.item.id == item.id)
            {
                amountFound += itemSlot.item.amount;
            }
        }

        return amountFound >= requiredAmount;
    }
}