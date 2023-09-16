using Player.Stats.Templates;
using UnityEngine;

namespace Player.Stats.Persistent
{
    public class BattleStat : Stat
    {
        private float _changeCooldown;
        private float _maxChangeCooldown;
        
        public delegate void StatChange(float value);
        public event StatChange OnStatChange;

        public delegate void StatDeplete();
        public event StatDeplete OnStatDeplete;

        private GameObject _toKill;

        public BattleStat(string name, float maxValue, Stat persistentStat, float currValue) : base(name, maxValue)
        {
            Value = currValue;
            OnStatChange += persistentStat.SetValue;
        }

        public void SetMaxChangeCooldown(float maxChangeCooldown)
        {
            _maxChangeCooldown = maxChangeCooldown;
        }

        private void ResetCooldown()
        {
            _changeCooldown = _maxChangeCooldown;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (_changeCooldown <= 0) return;
            _changeCooldown -= deltaTime;
        }

        public void AddValue(float value, bool ignoreIFrames)
        {
            if (!ignoreIFrames && _changeCooldown > 0) return;
            Value += value;
            if (Value > BaseValue) Value = BaseValue;
            ResetCooldown();
            OnStatChange?.Invoke(Value);
        }
        
        public void MinusValue(float value, bool ignoreIFrames)
        {
            if (!ignoreIFrames && _changeCooldown > 0) return;
            Value -= value;
            if (Value < 0)
            {
                Value = 0;
                OnStatChange?.Invoke(Value);
                OnStatDeplete?.Invoke();
                GameObject tempKill = _toKill;
                _toKill = null;
                if(tempKill) tempKill.SetActive(false);
                return;
            }
            ResetCooldown();
            OnStatChange?.Invoke(Value);
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