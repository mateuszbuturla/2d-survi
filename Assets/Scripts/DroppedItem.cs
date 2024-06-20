using System.Collections;
using TMPro;
using UnityEngine;

public class DroppedItem : MonoBehaviour, IInteractable
{
    public GameObject item;
    public static int despawnTimeout = 300;
    public static float textFadeoutTime = 0.5f; //unused for now
    public SpriteRenderer spriteRenderer;
    public TextMeshPro interactableText;

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

    IEnumerator AlphaDecay()
    {
        interactableText.color = new Color(interactableText.color.r,interactableText.color.g,interactableText.color.b,1);
        if (Singleton.instance.players[0].GetComponent<PlayerController>().detectedObject == this.gameObject) { yield break; }
        while (interactableText.color.a > 0)
        {
            interactableText.color = new Color(
                interactableText.color.r, 
                interactableText.color.g, 
                interactableText.color.b, 
                interactableText.color.a - 0.01f);
            yield return 0;
        }
    }

    public void ShowInteractionText()
    {
        StartCoroutine(AlphaDecay());
        interactableText.text = $"Pick up {item.GetComponent<Item>().name} [{item.GetComponent<Item>().amount}] ('E')";
    }

    public void Interact(Player player)
    {
        if (player.playerInventory.AddItem(item.GetComponent<Item>()))
        {
            Destroy(this.gameObject);
        }
    }
}