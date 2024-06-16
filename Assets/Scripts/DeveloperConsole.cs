using System.Collections;
using UnityEngine;

public class DeveloperConsole : MonoBehaviour
{
    public static void CreateDroppedItem(string item)
    {
        GameObject droppedItem = Instantiate(Singleton.instance.droppedItemPrefab);
        AllItems.GetItem(item, ref droppedItem.GetComponent<DroppedItem>().item);
    }
}