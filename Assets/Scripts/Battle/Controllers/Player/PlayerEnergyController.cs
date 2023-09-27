using System;
using Battle.Hitboxes;
using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{    
    public class PlayerEnergyController : MonoBehaviour 
    {
        [SerializeField][Range(1, 2)] private int playerNum = 1;
        [SerializeField] private PlayerEnergyController otherPlayerEnergy;
        [SerializeField] private Hitbox[] energyAbsorbers;
        private BattleStat _energy;
        [SerializeField] private WingGlow wingGlow;

        public delegate void EnergyUpdate(float change);
        public event EnergyUpdate OnEnergyUpdate;
        
        void Awake()
        {
            _energy = PlayerStats.CreateBattleEnergyStat(playerNum);
        }
        
        private void OnEnable() 
        {
            SubscribeToEvents();
        }

        private void OnDisable() 
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnDamaged += AbsorbEnergy;
            }
        }

        private void UnsubscribeFromEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnDamaged -= AbsorbEnergy;
            }
        }

        private void AbsorbEnergy(float amount, DamageType damageType)
        {
            float finalAmt = PlayerStats.ApplyEnergyAbsorbMultiplier(playerNum, amount);
            AddEnergy(finalAmt);
            wingGlow.TurnOnGlow();
            otherPlayerEnergy.AddEnergy(finalAmt * PlayerStats.GetEnergyShare(playerNum == 1 ? 2 : 1));
        }

        private void AddEnergy(float amount)
        {
            _energy.AddValue(amount);
            OnEnergyUpdate?.Invoke(change:amount);
        }

        public bool HasEnergy(float amount)
        {
            return _energy.CanMinusValue(amount);
        }

        /// <summary>
        /// Uses specified energy amount.
        /// If amount is more than energy remaining, will use whatever energy is remaining.
        /// </summary>
        /// <param name="amount">amount of energy to be used</param>
        public void UseEnergy(float amount)
        {
            if (_energy.CanMinusValue(amount))
            {
                OnEnergyUpdate?.Invoke(change:-amount);
                _energy.MinusValue(amount);
            }
            else
            {
                // if not enough energy, use whatever energy is left.
                OnEnergyUpdate?.Invoke(-_energy.GetValue());
                _energy.MinusValue(_energy.GetValue());
            }
        }

        public float GetEnergyAmount()
        {
            return _energy.GetValue();
        }
    }
}