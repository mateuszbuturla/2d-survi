using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Entity, IDamagable
{
    public void TakeDamage(Entity source, int damage)
    {
        health -= damage;
        print("ow "+health);
    }
}
