using System;

public class ExponentialStat : Stat
{
    private readonly float _minValue;
    private readonly float _maxValue;

    public ExponentialStat(
        string name, int maxLevel,
        float baseValue, float changeValue, float minValue, float maxValue,
        int baseCost, int addCost) :
        base(name, maxLevel, baseValue, changeValue, baseCost, addCost)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }
    
    public new void SetLevel(int level)
    {
        Level = level;
        if (IsMaxLevel()) Level = MaxLevel;
        Value = BaseValue * (int) Math.Pow(ChangeValue, Level - 1);
        base.SetLevel(level);
    }

    public new void Upgrade()
    {
        if (IsMaxLevel()) return;
        base.Upgrade();
        var newValue = Value * ChangeValue;
        Value = newValue;
        if (Value < _minValue) Value = _minValue;
        if (Value > _maxValue) Value = _maxValue;
    }
}