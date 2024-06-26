using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : ProjectileWeapon
{
    public override void Use(Player player)
    {
        //Create projectile
        GameObject arrow = Instantiate(projectiles[0]);

        Projectile arrowProjectile = arrow.GetComponent<Projectile>();
        arrowProjectile.owner = player;
        arrowProjectile.projectileWeapon = this;

        arrow.transform.position = this.transform.position;


        //Get direction to cursor
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.rotation = rotation;

        //Add speed
        arrow.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed * Time.deltaTime);
    }
}
