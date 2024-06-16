using UnityEngine;

public interface IInteractable
{
    public string InteractionText();
    public void Interact(Player player);
}