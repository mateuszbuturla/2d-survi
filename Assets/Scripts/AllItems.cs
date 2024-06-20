using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItems : MonoBehaviour
{
    public List<GameObject> allItemsList;
    public Dictionary<string, GameObject> items = new();

    public void Start()
    {
        foreach (GameObject item in allItemsList)
        {
            items.Add(item.name, item);
        }
    }

    /// <summary>
    /// Get an item to attach to a GameObject of your choice.
    /// </summary>
    /// <param name="Name">Name of the item to get.</param>
    /// <param name="gameObject">Object to attach it to.</param>
    public static GameObject GetItem(string itemName)
    {
        return Singleton.instance.allItems.items.GetValueOrDefault(itemName);
    }

    public static Item GetItemComponent(string itemName)
    {
        return GetItem(itemName).GetComponent<Item>();
    }
}