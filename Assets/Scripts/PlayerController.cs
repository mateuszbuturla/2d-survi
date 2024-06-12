using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    public bool allowMovement = true;
    public Vector2 movementDirection = Vector2.zero;

    public float startingAccelerationBonusThreshold = 1f;
    public float startingAccelerationBonusMultiplier = 1.5f;

    private void Update()
    {
        movementDirection = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            movementDirection.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection.y -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDirection.x -= 1;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            player.UseHeldPrimary();
        }
        if (Input.GetMouseButtonDown(1))
        {
            player.UseHeldSecondary();
        }

        MovePlayer();
    }

    public void MovePlayer()
    {
        player.playerRigidbody.AddForce(GetPlayerAcceleration() * Time.deltaTime * movementDirection.normalized);
    }

    public float GetPlayerAcceleration()
    {
        if (Mathf.Abs(player.playerRigidbody.velocity.x) < startingAccelerationBonusThreshold && 
            Mathf.Abs(player.playerRigidbody.velocity.y) < startingAccelerationBonusThreshold)
        {
            return player.playerAcceleration * startingAccelerationBonusMultiplier;
        }
        else
        {
            return player.playerAcceleration;
        }
    }

}