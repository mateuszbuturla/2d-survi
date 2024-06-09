using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    public float weaponRange;
    public float projectileSpeed;

    public List<GameObject> projectiles;
}
