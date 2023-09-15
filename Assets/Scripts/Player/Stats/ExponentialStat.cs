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

    public new void Upgrade()
    {
        if (IsMaxLevel()) return;
        base.Upgrade();
        var newValue = _value * _changeValue;
        _value = newValue;
        if (_value < _minValue) _value = _minValue;
        if (_value > _maxValue) _value = _maxValue;
    }
}