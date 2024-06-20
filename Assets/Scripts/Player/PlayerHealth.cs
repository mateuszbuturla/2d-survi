using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    public UIBar healthBar;

    public override void Start()
    {
        base.Start();
        healthBar.SetupBar(0, maxHealth, currentHealth);
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
        healthBar.UpdateValue(currentHealth);
    }
}
