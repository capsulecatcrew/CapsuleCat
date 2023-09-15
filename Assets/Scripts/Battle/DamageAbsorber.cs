using System.Collections.Generic;
using UnityEngine;

public class DamageAbsorber: Damageable
{
    [SerializeField] [Range(0, 2)] private int playerNum;

    public new void TakeDamage(float damage, bool ignoreCooldown)
    {
        if (!ignoreCooldown && DamageCooldown > 0) return;
        PlayerStats.AbsorbEnergy(playerNum, damage);
        DamageCooldown = DamageBaseCooldown;
    }
}
