using Player.Stats.Persistent;

namespace Player.Stats.Templates
{
    public class Stat
    {
        public string Name { get; }

        protected internal float Value;
        protected float BaseValue;
        protected readonly bool IsHealthStat;

        public string Description { get; } = "";

        public delegate void StatReset(float value);

        public event StatReset OnStatReset;

        /// <summary>
        /// Creates a stat with baseValue as the reset value.
        /// </summary>
        /// <param name="name">Name of stat created.</param>
        /// <param name="baseValue">Value to reset stat to.</param>
        /// <param name="isHealthStat">Whether stat is a health stat.</param>
        /// <param name="description">Description of stat created.</param>
        public Stat(string name, float baseValue, bool isHealthStat, string description = "")
        {
            Name = name;
            Value = baseValue;
            BaseValue = baseValue;
            IsHealthStat = isHealthStat;
            Description = description;
        }

        /// <summary>
        /// Creates a stat with Value of maxStat as the reset value.
        /// </summary>
        /// <param name="name">Name of stat created.</param>
        /// <param name="maxStat">Value to reset stat to.</param>
        public Stat(string name, UpgradeableStat maxStat)
        {
            Name = name;
            BaseValue = maxStat.Value;
            Value = BaseValue;
            maxStat.OnStatUpdate += SetValue;
            maxStat.OnStatReset += OnMaxStatReset;
        }

        public void SetValue(float value)
        {
            Value = value;
        }

        private void SetValue(int ignored1, float value, int ignored2)
        {
            Value = Value/BaseValue * value;
            BaseValue = value;
        }

        public float GetValue()
        {
            return Value;
        }

        private void OnMaxStatReset(float value)
        {
            BaseValue = value;
            Reset();
        }

        /// <summary>
        /// Resets the stat's value to its base value.
        /// <p>Invokes OnStatReset event.</p>
        /// </summary>
        public virtual void Reset()
        {
            Value = BaseValue;
            OnStatReset?.Invoke(Value);
        }

        /// <summary>
        /// Creates a BattleStat from a normal Stat.
        /// <p> BattleStat name = Stat name</p>
        /// <p> BattleStat max value = Stat base value</p>
        /// <p> BattleStat value = Stat value</p>
        /// </summary>
        public BattleStat CreateBattleStat()
        {
            return new BattleStat(Name, BaseValue, this, Value, IsHealthStat);
        }
    }
}