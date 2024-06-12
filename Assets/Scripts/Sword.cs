using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MeleeWeapon
{
    public int swingAngle = 120;
    public float distanceFromPlayer = 1;

    public override void PrimaryUseEffect(Player player)
    {
        // -- Create projectile
        GameObject sword = Instantiate(projectiles[0]);
        sword.transform.SetParent(player.playerPivot.transform);

        MeleeProjectile swordProjectile = sword.GetComponent<MeleeProjectile>();
        swordProjectile.owner = player;
        swordProjectile.meleeWeapon = this;

        int swingDirection = RandomSign();

        // -- Get direction to cursor
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // rotate by 180, otherwise swings behind back
        player.playerPivot.transform.rotation = rotation;
        sword.transform.rotation = rotation;
        sword.transform.position = player.playerPivot.transform.position;
        sword.transform.position += player.playerPivot.transform.up;

        // -- Start swinging async
        StartCoroutine(SwingSword(player.playerPivot, sword, rotation, swingDirection, swingAngle));

        StartPrimaryUseCooldown();
    }

    public override void SecondaryUseEffect(Player player) { }

    IEnumerator SwingSword(GameObject pivot, GameObject sword, Quaternion rotation, int swingDirection, int swingAngle)
    {
        // multiplying a quaternion is like adding in euler...
        Quaternion startRotation = rotation * Quaternion.Euler(0, 0, -swingAngle/2 * swingDirection);
        // negative swing direction: if start at -45, move towards +45 etc.
        Quaternion endRotation = rotation * Quaternion.Euler(0, 0, swingAngle/2 * swingDirection);

        //hopefully, loop lasts as long as cooldown
        for (float i = 0; i <= primaryUseCooldown; i += Time.deltaTime)
        {
            // again, multiplying quaternions is supposedly like adding euler
            pivot.transform.rotation = Quaternion.Lerp(startRotation, endRotation, i / primaryUseCooldown);

            sword.transform.position = pivot.transform.position;
            sword.transform.position += pivot.transform.up;

            yield return 0;
        }

        //pivot.transform.SetPositionAndRotation(pivot.transform.position, Quaternion.identity);
        sword.GetComponent<MeleeProjectile>().Deactivate();

        yield return null;
    }



    // -- Helper
    int RandomSign(){
        return Random.value < .5? 1 : -1;
    }

}
