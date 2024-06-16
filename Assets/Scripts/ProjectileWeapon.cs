using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileWeapon : Weapon
{
    public float weaponRange;

    public List<GameObject> projectiles;
}
