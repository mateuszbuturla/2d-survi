using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Entity, IDamagable
{
    public float health = 20;

    public void TakeDamage(Entity source, float damage)
    {
        health -= damage;
        print("ow "+health);
    }
}
