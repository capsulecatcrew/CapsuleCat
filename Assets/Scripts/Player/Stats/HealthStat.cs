public class HealthStat
{
    private float _health;

    public delegate void HealthUpdate(float value);
    public delegate void DeathUpdate();

    public event HealthUpdate OnDamageUpdate;
    public event HealthUpdate OnHealUpdate;
    public event DeathUpdate OnDeath;

    private LinearStat _statMaxHealth;

    public HealthStat(LinearStat statMaxHealth)
    {
        _statMaxHealth = statMaxHealth;
        _health = statMaxHealth.GetValue();
    }

    public void UpdateMaxHealth(LinearStat statMaxHealth)
    {
        _statMaxHealth = statMaxHealth;
    }

    public void Reset()
    {
        _health = _statMaxHealth.GetValue();
    }
    
    public void Damage(float amount)
    {
        if (amount > _health) return;
        _health -= amount;
        OnDamageUpdate?.Invoke(amount);
        if (_health > 0) return;
        OnDeath?.Invoke();
    }

    public void Heal(float amount)
    {
        if (float.MaxValue - amount < _health) return;
        var newHealth = _health + amount;
        var oldHealth = _health;
        if (!_statMaxHealth.IsWithinBounds(newHealth))
        {
            newHealth = _statMaxHealth.GetValue();
        }
        _health = newHealth;
        OnHealUpdate?.Invoke(_health - oldHealth);
    }

    public float GetHealthPercentage()
    {
        return _health / _statMaxHealth.GetValue();
    }
}