using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public GameObject item;
    public static int despawnTimeout = 300;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        StartCoroutine(DespawnTimeout());
    }

    public void RefreshSprite()
    {
        spriteRenderer.sprite = item.GetComponent<Item>().sprite;
    }

    IEnumerator DespawnTimeout()
    {
        yield return new WaitForSeconds(despawnTimeout);
        Destroy(this.gameObject);
    }

    public string InteractionText()
    {
        return $"Pick up {item.name} [{item.GetComponent<Item>().amount}] ('E')";
    }

    public void Interact(Player player)
    {
        if (player.inventoryItemGrid.GetComponent<Inventory>().AddItem(item.GetComponent<Item>()))
        {
            Destroy(this.gameObject);
        }
    }

    // -- Idk how to do this yet, leave this for later when it becomes a problem
    //static void GroupDropped()
    //{

    //}
}