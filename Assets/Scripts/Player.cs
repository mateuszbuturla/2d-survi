using UnityEngine;

public class Player : Entity, IDamagable
{
    public Holdable currentHeld;

    public GameObject playerPivot;
    public PlayerController playerController;
    public Rigidbody2D playerRigidbody;
    public GameObject inventoryWindow;
    public GameObject inventoryItemGrid;
    public GameObject inventoryItemHotbar;
    public GameObject interactableText;
    public GameObject developerConsole;

    private void Update()
    {
        currentHeld.transform.position = transform.position;
    }

    public void UseHeldPrimary()
    {
        if (currentHeld is Holdable)
        {
            currentHeld.UsePrimary(this);
        }
    }

    public void UseHeldSecondary()
    {
        if (currentHeld is Holdable)
        {
            currentHeld.UseSecondary(this);
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
