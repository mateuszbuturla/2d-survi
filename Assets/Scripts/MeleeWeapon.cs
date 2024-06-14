using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public float attackDuration;

    public List<GameObject> projectiles;
}
