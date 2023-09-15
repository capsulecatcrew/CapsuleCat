using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private HealthStat _health;

    protected const float DamageBaseCooldown = 1;
    protected float DamageCooldown;

    public AudioSource damageSoundSource;
    public AudioClip damageSound;

    // Update is called once per frame
    protected void Update()
    {
        if (DamageCooldown <= 0) return;
        DamageCooldown -= Time.deltaTime;
    }

    public void TakeDamage(float damage, bool ignoreCooldown)
    {
        if (!ignoreCooldown && DamageCooldown > 0) return;
        _health.Damage(damage);
        DamageCooldown = DamageBaseCooldown;
        
        if (damageSoundSource == null || !damageSoundSource.isActiveAndEnabled || damageSound == null) return;
        damageSoundSource.PlayOneShot(damageSound);
    }

    public void SetHealthSet(HealthStat healthStat)
    {
        _health = healthStat;
    }

    public HealthStat GetHealthStat()
    {
        return _health;
    }
}
