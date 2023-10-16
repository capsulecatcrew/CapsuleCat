using System.Text;

namespace Player.Stats.Templates
{
    public class UpgradeableStat : Stat
    {
        public int Level { get; private set; } = 1;
        protected readonly int MaxLevel;
        
        protected readonly float ChangeValue;

        public int Cost { get; private set; }
        private readonly int _baseCost;
        private readonly int _changeCost;

        public delegate void StatUpdate(int level, float value, int cost);
        public event StatUpdate OnStatUpdate;
        
        protected UpgradeableStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost, bool isHealthStat, string description = "") :
            base (name, baseValue, isHealthStat, description)
        {
            MaxLevel = maxLevel;
            ChangeValue = changeValue;
            Cost = baseCost;
            _baseCost = baseCost;
            _changeCost = changeCost;
        }
        
        /// <summary>
        /// Resets the stat's level to 1.
        /// <p>Resets the stat's value to base value.</p>
        /// <p>Resets the stat's cost to base cost.</p>
        /// <p>Invokes OnStatReset, OnStatUpdate events.</p>
        /// </summary>
        public override void Reset()
        {
            Level = 1;
            Cost = _baseCost;
            base.Reset();
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }
        
        /// <summary>
        /// Upgrades the stat's level, value and cost.
        /// <p> Does not upgrade if stat is at max level.</p>
        /// <p>Invokes OnStatUpdate event if not at max level.</p>
        /// </summary>
        public virtual void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            Level++;
            Cost += _changeCost;
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }

        /// <summary>
        /// Sets the stat's level, value and cost.
        /// <p>Clamps the values to max level.</p>
        /// <p>Invokes OnStatUpdate event.</p>
        /// </summary>
        public virtual void SetLevel(int level)
        {
            Level = level;
            if (IsMaxLevel()) Level = MaxLevel;
            Cost = _baseCost + (Level - 1) * _changeCost;
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }
        
        protected bool IsMaxLevel()
        {
            return Level >= MaxLevel;
        }

    }
}