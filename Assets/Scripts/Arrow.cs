using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderObject = collision.collider.gameObject;

        if (colliderObject.GetComponent<Entity>() is IDamagable)
        {
            (colliderObject.GetComponent<Entity>() as IDamagable).TakeDamage(null, projectileWeapon.weaponDamage);
        }

        //softdelete on collision
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Vector2.Distance(this.transform.position, owner.transform.position) > projectileWeapon.weaponRange)
        {
            //softdelete on maxrange
            this.gameObject.SetActive(false);
        }
    }
}
