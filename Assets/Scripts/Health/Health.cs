using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;

    private void Start()
    {
        OnStart();
    }

    public void ApplyDamage(Damage damage)
    {
        if (damage.damageType == DamageType.HEALING)
        {
            HandleHealing(damage);
            return;
        }

        HandleDamaging(damage);
    }

    private void HandleDamaging(Damage damage)
    {
        int damageAmount = damage.baseDamage;

        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            HandleDeath();
        }

        HandleHealthChange();
    }

    private void HandleHealing(Damage damage)
    {
        int healingAmount = damage.baseDamage;

        currentHealth += healingAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        HandleHealthChange();
    }

    public virtual void OnStart() { }
    public virtual void HandleDeath() { }
    public virtual void HandleHealthChange() { }
}
