using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public Slider healthBar;

    public override void OnStart()
    {
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public override void HandleHealthChange()
    {
        UpdateHealthBar();
    }

    public override void HandleDeath()
    {
        Debug.Log("Player is dead");
    }

    private void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ApplyDamage(new Damage(DamageType.DAMAGING, 10));
        }
    }
}
