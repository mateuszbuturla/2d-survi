using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public Item item;
    public static int despawnTimeout = 300;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer.sprite = item.sprite;
        StartCoroutine(DespawnTimeout());
    }

    IEnumerator DespawnTimeout()
    {
        yield return new WaitForSeconds(despawnTimeout);
        Destroy(this.gameObject);
    }

    public string InteractionText()
    {
        return $"Pick up {item.name} [{item.amount}] ('E')";
    }

    public void Interact(Player player)
    {
        if (player.inventoryItemGrid.GetComponent<Inventory>().AddItem(item))
        {
            Destroy(this.gameObject);
        }
    }

    // -- Idk how to do this yet, leave this for later when it becomes a problem
    //static void GroupDropped()
    //{

    //}
}