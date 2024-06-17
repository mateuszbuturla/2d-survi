using System.Collections;
using TMPro;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;

    [Header("Movement")]
    public bool allowMovement = true;
    public Vector2 movementDirection = Vector2.zero;
    public float startingAccelerationBonusThreshold = 1f;
    public float startingAccelerationBonusMultiplier = 1.5f;

    [Header("Detection")]
    //Detection Point
    public Transform detectionPoint;
    // -- Item pickup detection radius
    public float detectionRadius = 1f;
    // -- Detection Layer
    public LayerMask detectionLayer;
    // -- Cached Trigger Object
    public GameObject detectedObject;
    public static float detectionFrequency = .5f;

    void Start()
    {
        StartCoroutine(DetectInteractables());

        // -- Should start off, but stuff needs to load there, so it has to run at least once
        player.inventoryWindow.SetActive(false);
    }

    private void Update()
    {
        movementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.Interact();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            player.inventoryWindow.SetActive(!player.inventoryWindow.activeSelf);
            // -- Active hotbar only when inventoryWindow inactive
            player.inventoryItemHotbar.SetActive(!player.inventoryWindow.activeSelf);
        }

        MovePlayer();
    }

    public void MovePlayer()
    {
        if (!allowMovement) { return; }
        player.playerRigidbody.AddForce(GetPlayerAcceleration() * Time.deltaTime * movementDirection.normalized);
    }

    public float GetPlayerAcceleration()
    {
        if (Mathf.Abs(player.playerRigidbody.velocity.x) < startingAccelerationBonusThreshold &&
            Mathf.Abs(player.playerRigidbody.velocity.y) < startingAccelerationBonusThreshold)
        {
            return player.acceleration * startingAccelerationBonusMultiplier;
        }
        else
        {
            return player.acceleration;
        }
    }

    IEnumerator DetectInteractables()
    {
        while (true)
        {
            Collider2D collider = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

            if (collider != null && collider.gameObject.GetComponent<IInteractable>() != null)
            {
                detectedObject = collider.gameObject;
                player.interactableText.GetComponent<TextMeshProUGUI>().text = collider.gameObject.GetComponent<IInteractable>().InteractionText();
            }
            else
            {
                detectedObject = null;
                player.interactableText.GetComponent<TextMeshProUGUI>().text = "";
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}