using System;

namespace Player.Stats.Persistent
{
    public class ClampedLinearStat : UpgradeableLinearStat
    {
        private readonly float _minClamp;
        private readonly float _maxClamp;
        
        public delegate void ClampedStatUpdate(float value);
        public event ClampedStatUpdate OnClampedStatUpdate;

        public ClampedLinearStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost,
            float minClamp, float maxClamp, bool isHealthStat) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat)
        {
            _minClamp = minClamp;
            _maxClamp = maxClamp;
        }
        
        public override void UpgradeLevel()
        {
            base.UpgradeLevel();
            Value = Math.Clamp(Value, _minClamp, _maxClamp);
            OnClampedStatUpdate?.Invoke(Value);
        }
        
        public override void SetLevel(int level)
        {
            base.SetLevel(level);
            Value = Math.Clamp(Value, _minClamp, _maxClamp);
            OnClampedStatUpdate?.Invoke(Value);
        }
    }
}