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
                itemSlot.AcceptItem(item, null);
                return true;
            }
        }
        return false;
    }
}