using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Holdable, IUsable
{
    public int weaponDamage;

    public abstract void Use(Player player);
}
