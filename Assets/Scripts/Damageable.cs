using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public enum DamageType
    {
        Bullet,
        Laser
    }
    
    public int maxHp = 10;

    public int currentHp = 10;

    public float invincibilitySeconds = 0;

    public AudioSource damageSoundSource;
    public AudioClip damageSound;
    
    protected float TimeSinceLastHit;
    public delegate void HealthUpdate(Damageable damageable);

    public event HealthUpdate OnDamage;

    public event HealthUpdate OnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        TimeSinceLastHit = 0.0f;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (invincibilitySeconds > 0 && TimeSinceLastHit < invincibilitySeconds)
        {
            TimeSinceLastHit += Time.deltaTime;
        }
    }

    public virtual void TakeDamage(int damage, bool ignoreInvincibility = false, DamageType damageType = DamageType.Bullet)
    {
        if (ignoreInvincibility || TimeSinceLastHit > invincibilitySeconds)
        {
            currentHp -= damage;
            OnDamage?.Invoke(this);
            if (currentHp <= 0)
            {
                currentHp = 0;
                OnDeath?.Invoke(this);
            }
            
            if (invincibilitySeconds > 0.0f) TimeSinceLastHit = 0.0f;

            if (damageSoundSource != null && damageSoundSource.isActiveAndEnabled && damageSound != null)
            {
                damageSoundSource.PlayOneShot(damageSound);
            }
            
        }

    }

    public void SetMaxHp(int value)
    {
        maxHp = value;
    }
    
    public void SetCurrentHp(int value)
    {
        currentHp = value;
    }
}
