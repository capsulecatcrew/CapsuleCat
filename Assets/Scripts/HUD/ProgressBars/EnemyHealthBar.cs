public class EnemyHealthBar : HealthBar
{
    public void SetStats(LinearStat statMaxHealth, HealthStat statHealth)
    {
        StatMaxHealth = statMaxHealth;
        StatMaxHealth.OnStatUpdate += SetMaxValue;
        SetMaxValue(0, StatMaxHealth.GetValue(), 0);
        StatHealth = statHealth;
        StatHealth.OnDamageUpdate += MinusValue;
        StatHealth.OnHealUpdate += AddValue;
        SetValue(StatMaxHealth.GetValue());
    }
    public void OnDisable()
    {
        StatMaxHealth.OnStatUpdate -= SetMaxValue;
        StatHealth.OnDamageUpdate -= MinusValue;
        StatHealth.OnHealUpdate -= AddValue;
    }
}