using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float speed = 5f; // Prędkość ruchu postaci
    private Vector3 movement;

    void Update()
    {
        // Odczytanie wejścia gracza
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Ruch postaci
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }
}
