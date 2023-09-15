public class SplitSpecialStats
{
    private LinearStat SpecialAbsorb = new LinearStat("Special Absorb Rate", 10, 0.4f, 0.02f, 50, 75);
    private LinearStat SpecialDamage = new LinearStat("Special Damage Rate", 10, 0.2f, 0.02f, 50, 75);
    private LinearStat SpecialDamaged = new LinearStat("Special Damaged Rate", 10, 1.0f, 0.1f, 50, 75);
    private LinearStat SpecialDrainRate = new LinearStat("Special Drain Rate", 10, 1.0f, -0.1f, 50, 75);

    public void Reset()
    {
        SpecialAbsorb.Reset();
        SpecialDamage.Reset();
        SpecialDamaged.Reset();
        SpecialDrainRate.Reset();
    }

    public void Upgrade()
    {
        SpecialAbsorb.Upgrade();
        SpecialDamage.Upgrade();
        SpecialDamaged.Upgrade();
        SpecialDrainRate.Upgrade();
    }

    public float GetAbsorbValue()
    {
        return SpecialAbsorb.GetValue();
    }

    public float GetDamageValue()
    {
        return SpecialDamage.GetValue();
    }

    public float GetDamagedValue()
    {
        return SpecialDamaged.GetValue();
    }

    public float GetDrainRate()
    {
        return SpecialDrainRate.GetValue();
    }

    public int GetCost()
    {
        return SpecialAbsorb.GetCost();
    }
}