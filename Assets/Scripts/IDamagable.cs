using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public abstract void TakeDamage(Entity source, int damage);
}