using UnityEngine;

public class Player : Entity, IDamagable
{
    public GameObject playerPivot;
    public PlayerController playerController;
    public Rigidbody2D playerRigidbody;
    public GameObject inventoryWindow;
    public Inventory playerInventory;
    public Inventory playerArmor;
    public Inventory playerTrinkets;
    public GameObject inventoryItemHotbar;

    public void UseHeldPrimary()
    {
        if (HotbarItemSlot.selectedHotbarSlot == null) { return; }
        if (HotbarItemSlot.selectedHotbarSlot.item is Holdable)
        {
            (HotbarItemSlot.selectedHotbarSlot.item as Holdable).UsePrimary(this);
        }
    }

    public void UseHeldSecondary()
    {
        if (HotbarItemSlot.selectedHotbarSlot == null) { return; }
        if (HotbarItemSlot.selectedHotbarSlot.item is Holdable)
        {
            (HotbarItemSlot.selectedHotbarSlot.item as Holdable).UseSecondary(this);
        }
    }

    public void Interact()
    {
        if (playerController.detectedObject != null)
        {
            playerController.detectedObject.GetComponent<IInteractable>().Interact(this);
        }
    }

    public void TakeDamage(Entity source, int damage)
    {
        health -= damage;
    }
}
