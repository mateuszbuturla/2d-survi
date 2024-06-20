using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Useable : Item
{
    // Primary use cooldown
    public float primaryUseCooldown;
    public float currentPrimaryUseCooldown;
    // Secondary use cooldown
    public float secondaryUseCooldown;
    public float currentSecondaryUseCooldown;

    public virtual bool CanUsePrimary(Player player) { return true; }
    public abstract void PrimaryUseEffect(Player player);
    public virtual bool CanUseSecondary(Player player) { return true; }
    public abstract void SecondaryUseEffect(Player player);

    public void UsePrimary(Player player)
    {
        if (currentPrimaryUseCooldown <= 0 && CanUsePrimary(player))
        {
            PrimaryUseEffect(player);
        }
    }

    public void UseSecondary(Player player)
    {
        if (currentSecondaryUseCooldown <= 0 && CanUseSecondary(player))
        {
            SecondaryUseEffect(player);
        }
    }

    protected void StartPrimaryUseCooldown()
    {
        currentPrimaryUseCooldown = primaryUseCooldown;
    }

    protected void StartSecondaryUseCooldown()
    {
        currentSecondaryUseCooldown = secondaryUseCooldown;
    }

    protected void ReduceDurability()
    {
        remainingDurability--;
        if (remainingDurability <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (currentPrimaryUseCooldown > 0)
        {
            currentPrimaryUseCooldown -= Time.deltaTime;
        }

        if (currentSecondaryUseCooldown > 0)
        {
            currentSecondaryUseCooldown -= Time.deltaTime;
        }
    }
}
