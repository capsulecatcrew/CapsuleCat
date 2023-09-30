using Player.Stats.Templates;
using UnityEngine;

namespace Player.Stats.Persistent
{
    public class BattleStat : Stat
    {
        private bool _hasDepleted;
        private readonly Stat _persistentStat;

        public delegate void StatChange(float value);
        public event StatChange OnStatChange;

        public delegate void StatDeplete();
        public event StatDeplete OnStatDeplete;

        public BattleStat(string name, float maxValue, Stat persistentStat, float currValue, bool isHealthStat) :
            base(name, maxValue, isHealthStat)
        {
            Value = currValue;
            _persistentStat = persistentStat;
        }

        public void AddValue(float value)
        {
            var oldValue = Value;
            Value += value;
            if (Value > BaseValue) Value = BaseValue;
            OnStatChange?.Invoke(Value - oldValue);
            _persistentStat.SetValue(Value);
        }

        public bool MinusValue(float value)
        {
            var oldValue = Value;
            Value -= value;
            if (Value < 0)
            {
                Value = 0;
                OnStatChange?.Invoke(-oldValue);
                _persistentStat.SetValue(Value);
                if (IsHealthStat && _hasDepleted) return false;
                OnStatDeplete?.Invoke();
                _hasDepleted = true;
                return true;
            }
            OnStatChange?.Invoke(-value);
            _persistentStat.SetValue(Value);
            return true;
        }

        public bool CanMinusValue(float value)
        {
            return Value >= value;
        }

        public float GetStatPercentage()
        {
            return Value / BaseValue;
        }
    }
}