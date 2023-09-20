using Player.Stats.Templates;
using UnityEngine;

namespace Player.Stats.Persistent
{
    public class RandomStat : UpgradeableStat
    {
        private UpgradeableStat _stat1;
        private UpgradeableStat _stat2;
        
        protected RandomStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost, bool isHealthStat) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat) { }
        
        public RandomStat(UpgradeableStat stat1, UpgradeableStat stat2) :
            base("ignored", 0, 0, 0, 0, 0, false)
        {
            _stat1 = stat1;
            _stat2 = stat2;
        }

        public float GenerateRandomValue()
        {
            return Random.Range(_stat1.Value, _stat2.Value);
        }
    }
}