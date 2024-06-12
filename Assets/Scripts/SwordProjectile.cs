using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectile : MeleeProjectile
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliderObject = collision.gameObject;

        if (colliderObject.GetComponent<Entity>() is IDamagable)
        {
            (colliderObject.GetComponent<Entity>() as IDamagable).TakeDamage(null, meleeWeapon.weaponDamage);
        }
    }
}
