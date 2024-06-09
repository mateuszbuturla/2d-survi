using UnityEngine;

public class Player : Entity, IDamagable
{
    public float playerHealth;

    public float playerMovementSpeed;
    public float playerAcceleration;

    public Holdable currentHeld;

    public Camera playerCamera;
    public PlayerController playerController;
    public Rigidbody2D playerRigidbody;

    private void Update()
    {
        playerCamera.transform.position = new Vector3(transform.position.x,transform.position.y,playerCamera.transform.position.z);

        // TEMPORARY
        currentHeld.transform.position = transform.position;
    }

    public void UseItem()
    {
        if (currentHeld is IUsable)
        {
            (currentHeld as IUsable).Use(this);
        }
    }

    public void TakeDamage(Entity source, float damage)
    {
        playerHealth -= damage;
    }
}
