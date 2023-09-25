using Player.Stats.Templates;

namespace Player.Stats.Persistent
{
    public class GroupedUpgradeableStat : UpgradeableStat
    {
        private readonly UpgradeableStat[] _stats;
        
        public GroupedUpgradeableStat(
            string name, int maxLevel, int baseCost, int changeCost, bool isHealthStat, params UpgradeableStat[] stats) :
            base(name, maxLevel, 0, 0, baseCost, changeCost, isHealthStat)
        {
            _stats = stats;
        }

        public override void Reset()
        {
            foreach (var stat in _stats)
            {
                stat.Reset();
            }
            base.Reset();
        }

        public override void UpgradeLevel()
        {
            foreach (var stat in _stats)
            {
                stat.UpgradeLevel();
            }
            base.UpgradeLevel();
        }
        
        public override void SetLevel(int level)
        {
            foreach (var stat in _stats)
            {
                stat.SetLevel(level);
            }
            base.SetLevel(level);
        }
    }
}