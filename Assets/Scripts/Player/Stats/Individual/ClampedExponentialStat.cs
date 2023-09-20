using System;
using Player.Stats.Templates;

namespace Player.Stats.Persistent
{
    public class ClampedExponentialStat : UpgradeableExponentialStat
    {
        private readonly float _minClamp;
        private readonly float _maxClamp;
        
        public delegate void ClampedStatUpdate(float value);
        public event ClampedStatUpdate OnClampedStatUpdate;

        public ClampedExponentialStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost,
            float minClamp, float maxClamp, bool isHealthStat) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat)
        {
            _minClamp = minClamp;
            _maxClamp = maxClamp;
        }
        
        /// <summary>
        /// Upgrades the stat's level, value and cost.
        /// <p> Does not upgrade if stat is at max level.</p>
        /// <p>Invokes OnStatUpdate event if not at max level.</p>
        /// </summary>
        public new void UpgradeLevel()
        {
            base.UpgradeLevel();
            Value = Math.Clamp(Value, _minClamp, _maxClamp);
            OnClampedStatUpdate?.Invoke(Value);
        }

        /// <summary>
        /// Sets the stat's level, value and cost.
        /// <p>Clamps the values to max level.</p>
        /// <p>Invokes OnStatUpdate event.</p>
        /// </summary>
        public new void SetLevel(int level)
        {
            base.SetLevel(level);
            Value = Math.Clamp(Value, _minClamp, _maxClamp);
            OnClampedStatUpdate?.Invoke(Value);
        }
    }
}