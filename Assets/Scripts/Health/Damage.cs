public class Damage
{
    public DamageType damageType;
    public int baseDamage;
    public bool isCrit;

    public Damage(DamageType damageType, int baseDamage, bool isCrit = false)
    {
        this.damageType = damageType;
        this.baseDamage = baseDamage;
        this.isCrit = isCrit;
    }
}