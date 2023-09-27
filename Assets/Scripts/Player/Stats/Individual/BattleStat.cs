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

        public delegate void StatIncrease();
        public event StatIncrease OnStatIncrease;

        public delegate void StatDecrease();
        public event StatDecrease OnStatDecrease;

        public delegate void StatDeplete();
        public event StatDeplete OnStatDeplete;

        private GameObject _toKill;

        public BattleStat(string name, float maxValue, Stat persistentStat, float currValue, bool isHealthStat) :
            base(name, maxValue, isHealthStat)
        {
            Value = currValue;
            _persistentStat = persistentStat;
        }

        public bool AddValue(float value)
        {
            Value += value;
            if (Value > BaseValue) Value = BaseValue;
            OnStatChange?.Invoke(Value);
            _persistentStat.SetValue(Value);
            OnStatIncrease?.Invoke();
            return true;
        }

        public bool MinusValue(float value)
        {
            Value -= value;
            if (Value < 0)
            {
                Value = 0;
                OnStatChange?.Invoke(Value);
                _persistentStat.SetValue(Value);
                OnStatDecrease?.Invoke();
                if (IsHealthStat && _hasDepleted) return false;
                OnStatDeplete?.Invoke();
                _hasDepleted = true;
                var tempKill = _toKill;
                _toKill = null;
                if (tempKill) tempKill.SetActive(false);
                return true;
            }
            OnStatChange?.Invoke(Value);
            _persistentStat.SetValue(Value);
            OnStatDecrease?.Invoke();
            return true;
        }

        public bool CanMinusValue(float value)
        {
            return Value >= value;
        }

        public void SetGameObjectToKill(GameObject gameObject)
        {
            _toKill = gameObject;
        }

        public float GetStatPercentage()
        {
            return Value / BaseValue;
        }
    }
}