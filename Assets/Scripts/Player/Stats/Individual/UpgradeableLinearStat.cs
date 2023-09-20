using Player.Stats.Templates;
using Stats.Templates;

namespace Player.Stats.Persistent
{
    public class UpgradeableLinearStat : UpgradeableStat
    {
        public UpgradeableLinearStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost, bool isHealthStat = false) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat) { }

        public new void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            Value = LinearStatValues.GetNextLevelValue(Value, ChangeValue);
            base.UpgradeLevel();
        }

        public new void SetLevel(int level)
        {
            Value = LinearStatValues.GetSetLevelValue(Level, MaxLevel, BaseValue, ChangeValue);
            base.SetLevel(level);
        }

        public float ApplyValue(float value)
        {
            return value * Value;
        }
    }
}