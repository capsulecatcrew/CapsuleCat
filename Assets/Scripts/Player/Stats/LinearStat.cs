public class LinearStat : Stat
{
    public LinearStat(string name, int maxLevel, float baseValue, float changeValue, int baseCost, int addCost) :
        base(name, maxLevel, baseValue, changeValue, baseCost, addCost) { }

    public new void Upgrade()
    {
        if (IsMaxLevel()) return;
        base.Upgrade();
        _value += _changeValue;
    }
}