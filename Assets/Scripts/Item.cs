using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    public void LoadBaseStats(ItemSO itemSO)
    {
        this.id = itemSO.id;
        this.name = itemSO.name;
        this.type = itemSO.type;
        this.rarity = itemSO.rarity;
        this.sprite = itemSO.sprite;
        this.maxStack = itemSO.maxStack;
        this.hasDurability = itemSO.hasDurability;
        this.maxDurability = itemSO.maxDurability;
        this.remainingDurability = itemSO.maxDurability;

    }

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
}
