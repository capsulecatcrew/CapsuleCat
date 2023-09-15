public class SpecialStat
{
    private float _special;
    private const float SpecialMax = 50;
    private readonly SplitSpecialStats _splitSpecialStats;
    private ProgressBarManager _progressBarManager;
    
    public delegate void SpecialUpdate(float value);

    public event SpecialUpdate OnGainUpdate;
    public event SpecialUpdate OnUseUpdate;

    public SpecialStat(SplitSpecialStats splitSpecialStats, EnergyStat energyStat)
    {
        _splitSpecialStats = splitSpecialStats;
        energyStat.OnAbsorbUpdate += Absorb;
    }

    public float GetMaxValue()
    {
        return SpecialMax;
    }

    public void Reset(bool isLoss)
    {
        if (isLoss) _splitSpecialStats.Reset();
        _special = 0;
    }
    
    public bool Use(float amount)
    {
        if (amount > _special) return false;
        _special -= amount;
        OnUseUpdate?.Invoke(amount);
        return true;
    }

    public void Absorb(float amount)
    {
        var absorbAmount = amount * _splitSpecialStats.GetAbsorbValue();
        Gain(absorbAmount);
    }

    public void Damage(float amount)
    {
        var damageAmount = amount * _splitSpecialStats.GetDamageValue();
        Gain(damageAmount);
    }

    public void Damaged(float amount)
    {
        var damagedAmount = amount * _splitSpecialStats.GetDamagedValue();
        Gain(damagedAmount);
    }

    private void Gain(float amount)
    {
        if (float.MaxValue - amount < _special) return;
        var newSpecial = _special + amount;
        var oldSpecial = _special;
        if (newSpecial <= SpecialMax)
        {
            newSpecial = SpecialMax;
        }
        _special = newSpecial;
        OnGainUpdate?.Invoke(_special - oldSpecial);
    }
}