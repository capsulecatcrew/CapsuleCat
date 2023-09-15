public class SplitPlayerStats
{
    private int _money;
    
    private static readonly LinearStat StatDamage = new("Attack Damage", 10, 2, 1, 50, 75);
    
    private static readonly LinearStat StatMaxEnergy = new("Max Energy", 10, 30, 10, 50, 50);
    private static readonly LinearStat StatEnergyAbsorb = new("Energy Absorb", 10, 0.5f, 0.05f, 50, 75);
    private static readonly EnergyStat StatEnergy = new (StatMaxEnergy, StatEnergyAbsorb);
    
    private static readonly SplitSpecialStats StatSpecialStats = new();
    private static readonly SpecialStat StatSpecial = new (StatSpecialStats, StatEnergy);

    public string GetMoneyString()
    {
        return "$" + _money;
    }

    public void AddMoney(int amount)
    {
        if (int.MaxValue - amount < _money)
        {
            _money = int.MaxValue;
            return;
        }
        _money += amount;
    }

    public bool RemoveMoney(int amount)
    {
        if (amount > _money) return false;
        _money -= amount;
        return true;
    }

    public EnergyStat GetEnergyStat()
    {
        return StatEnergy;
    }

    public float GetDamage()
    {
        return StatDamage.GetValue();
    }

    public bool CanUseEnergy(float amount)
    {
        return StatEnergy.CanUse(amount);
    }

    public void UseEnergy(float amount)
    {
        StatEnergy.Use(amount);
    }

    public void AbsorbEnergy(float amount)
    {
        StatEnergy.Absorb(amount);
    }

    public void UpgradeDamage()
    {
        StatDamage.Upgrade();
    }

    public void UpgradeMaxEnergy()
    {
        StatMaxEnergy.Upgrade();
    }

    public void UpgradeEnergyAbsorb()
    {
        StatEnergyAbsorb.Upgrade();
    }

    public void UpgradeSpecialStats()
    {
        StatSpecialStats.Upgrade();
    }

    private void HardReset()
    {
        _money = 0;
        StatDamage.Reset();
    }

    public void Reset(bool isLoss)
    {
        if (isLoss) HardReset();
        StatEnergy.Reset(isLoss);
        StatSpecial.Reset(isLoss);
    }
}