using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbsorber: Damageable
{
    public List<DamageType> damageTypes;

    public float absorbMultiplier = 1.0f;
    
    public delegate void MyDelegate(float value);

    public event MyDelegate OnDamageAbsorb;
    
    public override void TakeDamage(int damage, bool ignoreInvincibility = false, DamageType damageType = DamageType.Bullet)
    {
        if (!ignoreInvincibility && TimeSinceLastHit < invincibilitySeconds) return;
        
        if (damageTypes.Contains(damageType))
        {
            OnDamageAbsorb?.Invoke(damage * absorbMultiplier);
            TimeSinceLastHit = 0.0f;
        }
        else
        {
            base.TakeDamage(damage, ignoreInvincibility, damageType);
        }
    }

}
