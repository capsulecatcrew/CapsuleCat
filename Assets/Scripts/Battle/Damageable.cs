using UnityEngine;

public class Damageable : MonoBehaviour
{
    protected HealthStat Health;

    protected const float DamageBaseCooldown = 1;
    protected float DamageCooldown;

    public AudioSource damageSoundSource;
    public AudioClip damageSound;

    // Update is called once per frame
    public void Update()
    {
        if (DamageCooldown <= 0) return;
        DamageCooldown -= Time.deltaTime;
    }

    public void TakeDamage(float damage, bool ignoreCooldown)
    {
        if (!ignoreCooldown && DamageCooldown > 0) return;
        Health.Damage(damage);
        DamageCooldown = DamageBaseCooldown;
        
        if (damageSoundSource == null || !damageSoundSource.isActiveAndEnabled || damageSound == null) return;
        damageSoundSource.PlayOneShot(damageSound);
    }

    public void SetHealthStat(HealthStat healthStat)
    {
        Health = healthStat;
    }

    public HealthStat GetHealthStat()
    {
        return Health;
    }
}
