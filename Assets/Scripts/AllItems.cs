using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AllItems
{
    public static Dictionary<string, Action<GameObject>> items = new Dictionary<string, Action<GameObject>>()
    {
        { "Bow", (GameObject obj) => { obj.AddComponent<Bow>(); } },
        { "Sword", (GameObject obj) => { obj.AddComponent<Sword>(); } }
    };

    /// <summary>
    /// Get an item to attach to a GameObject of your choice.
    /// </summary>
    /// <param name="Name">Name of the item to get.</param>
    /// <param name="gameObject">Object to attach it to.</param>
    public static void GetItem(string itemName, ref GameObject gameObject)
    {
        items.GetValueOrDefault(itemName)(gameObject);
    }
}