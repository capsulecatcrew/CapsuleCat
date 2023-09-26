using Player.Stats.Templates;
using UnityEngine;

namespace Player.Stats.Persistent
{
    public class BattleStat : Stat
    {
        private const float MaxChangeCooldown = 0.5f;
        private float _changeCooldown;
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
            OnStatChange += persistentStat.SetValue;
        }

        public void DecouplePersistentStat()
        {
            OnStatChange -= _persistentStat.SetValue;
        }

        private void ResetCooldown()
        {
            _changeCooldown = MaxChangeCooldown;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (_changeCooldown <= 0) return;
            _changeCooldown -= deltaTime;
        }

        public bool AddValue(float value, bool ignoreIFrames)
        {
            if (!ignoreIFrames && _changeCooldown > 0) return false;
            Value += value;
            if (Value > BaseValue) Value = BaseValue;
            ResetCooldown();
            OnStatChange?.Invoke(Value);
            OnStatIncrease?.Invoke();
            return true;
        }

        public bool MinusValue(float value, bool ignoreIFrames)
        {
            if (!ignoreIFrames && _changeCooldown > 0) return false;
            Value -= value;
            if (Value < 0)
            {
                Value = 0;
                OnStatChange?.Invoke(Value);
                OnStatDecrease?.Invoke();
                if (IsHealthStat && _hasDepleted) return false;
                OnStatDeplete?.Invoke();
                _hasDepleted = true;
                var tempKill = _toKill;
                _toKill = null;
                if (tempKill) tempKill.SetActive(false);
                return true;
            }
            ResetCooldown();
            OnStatChange?.Invoke(Value);
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