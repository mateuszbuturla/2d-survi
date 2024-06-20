using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Useable
{
    public int weaponDamage;

    // -- Primary/Secondary use cooldowns control this now... a little awkward in naming, but it's clean
    //public float weaponAttackSpeed; 
}
