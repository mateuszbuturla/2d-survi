using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeProjectile : MonoBehaviour
{
    public Entity owner;
    public MeleeWeapon meleeWeapon;

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
