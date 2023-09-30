using Player.Stats.Templates;
using Stats.Templates;

namespace Player.Stats.Persistent
{
    public class UpgradeableExponentialStat : UpgradeableStat
    {
        public UpgradeableExponentialStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost, bool isHealthStat) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat) { }

        public override void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            Value = ExponentialStatValues.GetNextLevelValue(Value, ChangeValue);
            base.UpgradeLevel();
        }

        public override void SetLevel(int level)
        {
            Value = ExponentialStatValues.GetSetLevelValue(Level, MaxLevel, BaseValue, ChangeValue);
            base.SetLevel(level);
        }
    }
}