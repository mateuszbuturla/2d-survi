using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    // Multiple, for weapons with alt attacks etc.
    public List<GameObject> projectiles;
}
