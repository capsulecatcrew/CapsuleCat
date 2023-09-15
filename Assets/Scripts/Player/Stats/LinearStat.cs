public class LinearStat : Stat
{
    public LinearStat(string name, int maxLevel, float baseValue, float changeValue, int baseCost, int addCost) :
        base(name, maxLevel, baseValue, changeValue, baseCost, addCost) { }

    public new void Upgrade()
    {
        if (IsMaxLevel()) return;
        base.Upgrade();
        Value += ChangeValue;
    }

    public new void SetLevel(int level)
    {
        Level = level;
        if (IsMaxLevel()) Level = MaxLevel;
        Value = BaseValue + (Level - 1) * ChangeValue;
        base.SetLevel(level);
    }
}