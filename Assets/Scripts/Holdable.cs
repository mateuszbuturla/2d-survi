using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Holdable : Item
{
    // Primary use cooldown
    public float primaryUseCooldown;
    public float currentPrimaryUseCooldown;
    // Secondary use cooldown
    public float secondaryUseCooldown;
    public float currentSecondaryUseCooldown;

    public abstract void PrimaryUseEffect(Player player);
    public abstract void SecondaryUseEffect(Player player);

    public void UsePrimary(Player player)
    {
        if (currentPrimaryUseCooldown <= 0) 
        {
            PrimaryUseEffect(player);
        }
    }

    public void UseSecondary(Player player)
    {
        if (currentSecondaryUseCooldown <= 0) 
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
