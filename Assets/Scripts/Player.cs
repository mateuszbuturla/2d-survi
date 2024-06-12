using UnityEngine;

public class Player : Entity, IDamagable
{
    public float playerHealth;

    public float playerMovementSpeed;
    public float playerAcceleration;

    public Holdable currentHeld;

    public Camera playerCamera;
    public GameObject playerPivot;
    public PlayerController playerController;
    public Rigidbody2D playerRigidbody;

    private void Update()
    {
        playerCamera.transform.position = new Vector3(transform.position.x,transform.position.y,playerCamera.transform.position.z);

        // TEMPORARY
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

    public void TakeDamage(Entity source, float damage)
    {
        playerHealth -= damage;
    }
}
