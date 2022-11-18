using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHp = 10;

    public int currentHp = 10;

    public float invincibilitySeconds = 0;
    private float _timeSinceLastHit;
    public delegate void HealthUpdate();

    public event HealthUpdate OnDamage;

    public event HealthUpdate OnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        _timeSinceLastHit = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibilitySeconds > 0 && _timeSinceLastHit < invincibilitySeconds)
        {
            _timeSinceLastHit += Time.deltaTime;
        }
    }

    public void TakeDamage(int damage, bool ignoreInvincibility = false)
    {
        if (ignoreInvincibility || _timeSinceLastHit > invincibilitySeconds)
        {
            currentHp -= damage;
            OnDamage?.Invoke();
            if (currentHp <= 0)
            {
                currentHp = 0;
                OnDeath?.Invoke();
            }
            
            if (invincibilitySeconds > 0.0f) _timeSinceLastHit = 0.0f;
            
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
