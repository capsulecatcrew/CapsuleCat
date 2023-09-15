using UnityEngine;

namespace HUD.ProgressBars
{
    public class HealthBar : ProgressBar
    {
        [SerializeField] private LinearStat _statMaxHealth;
        [SerializeField] private HealthStat _statHealth;
        
        public void Start()
        {
            _statMaxHealth.OnStatUpdate += SetMaxValue;
            _statHealth.OnDamageUpdate += MinusValue;
            _statHealth.OnHealUpdate += AddValue;
        }
    }
}