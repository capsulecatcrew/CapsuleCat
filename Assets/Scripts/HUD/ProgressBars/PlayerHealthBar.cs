using System;

public class PlayerHealthBar : HealthBar
{
    public void Awake()
    {
        StatMaxHealth = PlayerStats.GetMaxHealthStat();
        StatMaxHealth.OnStatUpdate += SetMaxValue;
        StatHealth = PlayerStats.GetHealthStat();
        StatHealth.OnDamageUpdate += MinusValue;
        StatHealth.OnHealUpdate += AddValue;
    }

    public void OnDisable()
    {
        StatMaxHealth.OnStatUpdate -= SetMaxValue;
        StatHealth.OnDamageUpdate -= MinusValue;
        StatHealth.OnHealUpdate -= AddValue;
    }
}