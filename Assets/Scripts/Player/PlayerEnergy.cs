// using System;
// using UnityEngine;
//
// public class PlayerEnergy
// {
//     [SerializeField] private static readonly SplitPlayerStats SplitPlayerStats;
//     [SerializeField] private DamageAbsorber energyAbsorber1;
//     [SerializeField] private DamageAbsorber energyAbsorber2;
//     
//     private float _maxValue;
//     private float _currentValue;
//
//     public delegate void EnergyUpdate(float amount, int playerNo);
//
//     public event EnergyUpdate OnEnergyUpdate;
//     
//     private void Start()
//     {
//         _maxValue = SplitPlayerStats.Energy.GetValue();
//         RefillToMax();
//         energyAbsorber1.SetAbsorbMultiplier(SplitPlayerStats.EnergyAbsorb.GetCurrentValue());
//         energyAbsorber2.SetAbsorbMultiplier(SplitPlayerStats.EnergyAbsorb.GetCurrentValue());
//         energyAbsorber1.OnDamageAbsorb += UpdateEnergy;
//         energyAbsorber2.OnDamageAbsorb += UpdateEnergy;
//     }
//
//
//     /// <summary>
//     /// Update player energy by specified amount.
//     /// </summary>
//     /// <param name="amount">Amount of energy to add to player.</param>
//     public void UpdateEnergy(float amount)
//     {
//         _currentValue += amount;
//         _currentValue = Math.Clamp(_currentValue, 0, _maxValue);
//         OnEnergyUpdate?.Invoke(_currentValue - amount, _playerNumber);
//     }
//
//     private void RefillToMax()
//     {
//         _currentValue = _maxValue;
//     }
//
//     public float GetCurrentAmount()
//     {
//         return _currentValue;
//     }
//
//     public bool HasEnergy(float amount)
//     {
//         return _currentValue >= amount;
//     }
// }
