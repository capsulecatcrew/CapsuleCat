namespace Player.Stats.Persistent
{
    public class IntervalLinearStat : UpgradeableLinearStat
    {
        private int _trueLevel;
        private readonly int _interval;

        public delegate void IntervalStatUpdate(float value);
        public event IntervalStatUpdate OnIntervalStatUpdate;

        public IntervalLinearStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost,
            int interval, bool isHealthStat) :
            base(name, maxLevel, baseValue, changeValue, baseCost, changeCost, isHealthStat)
        {
            _interval = interval;
        }
        
        public override void UpgradeLevel()
        {
            _trueLevel++;
            if ((0f + _trueLevel) / _interval - Level < float.Epsilon) return;
            base.UpgradeLevel();
            OnIntervalStatUpdate?.Invoke(Value);
        }
        
        public override void SetLevel(int level)
        {
            _trueLevel = level;
            if ((0f + _trueLevel) / _interval - Level < float.Epsilon) return;
            base.SetLevel(_trueLevel / _interval);
            OnIntervalStatUpdate?.Invoke(Value);
        }
    }
}