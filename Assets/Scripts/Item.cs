using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public GameObject prefab;
    public int id;
    public new string name;
    public Type type;
    public Rarity rarity;
    public Sprite sprite;

    public int maxStack;
    public int amount;

    public bool hasDurability;
    public int maxDurability;
    public int remainingDurability;

    public enum Type
    {
        NONE
    }

    public enum Rarity
    {
        NONE
    }

    protected ItemSlot GetContainingItemSlot()
    {
        return transform.parent.GetComponent<ItemSlot>();
    }

    protected Inventory GetContainingInventory()
    {
        return transform.parent.GetComponent<ItemSlot>().transform.parent.GetComponent<Inventory>();
    }
}
