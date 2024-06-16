using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Entity owner;
    public ProjectileWeapon projectileWeapon;

    public float projectileSpeed;
}
