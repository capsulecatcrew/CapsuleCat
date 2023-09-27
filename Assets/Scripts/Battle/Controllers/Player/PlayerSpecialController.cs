using Battle.Hitboxes;
using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{    
    public class PlayerSpecialController : MonoBehaviour 
    {
        [SerializeField][Range(1, 2)] private int playerNum = 1;
        [SerializeField] private Hitbox[] energyAbsorbers;
        [SerializeField] private Hitbox playerBody;
        private BattleStat _specialEnergy;

        public delegate void SpecialChange(float change);
        public event SpecialChange OnSpecialChange;
        
        void Awake()
        {
            _specialEnergy = PlayerStats.CreateBattleSpecialStat(playerNum);
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

            playerBody.OnDamaged += GetEnergyFromTakingDamage;
        }

        private void UnsubscribeFromEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnDamaged -= AbsorbEnergy;
            }
            
            playerBody.OnDamaged += GetEnergyFromTakingDamage;
        }

        private void AbsorbEnergy(float amount, DamageType damageType)
        {
            if (damageType == DamageType.Special)
            {
                AddSpecialEnergy(PlayerStats.ApplySpecialAbsorbMultipler(playerNum, amount));
            }
        }

        private void GetEnergyFromTakingDamage(float amount, DamageType damageType)
        {
            AddSpecialEnergy(PlayerStats.ApplySpecialDamagedMultipler(playerNum, amount));
        }

        private void AddSpecialEnergy(float amount)
        {
            _specialEnergy.AddValue(amount);
            OnSpecialChange?.Invoke(amount);
        }

        public bool HasSpecialEnergy(float amount)
        {
            return _specialEnergy.CanMinusValue(amount);
        }

        /// <summary>
        /// Uses specified special energy amount.
        /// If amount is more than energy remaining, will use whatever energy is remaining.
        /// </summary>
        /// <param name="amount">amount of energy to be used</param>
        public void UseSpecialEnergy(float amount)
        {
            if (_specialEnergy.CanMinusValue(amount))
            {
                OnSpecialChange?.Invoke(-amount);
                _specialEnergy.MinusValue(amount);
            }
            else
            {
                OnSpecialChange?.Invoke(-_specialEnergy.GetValue());
                _specialEnergy.MinusValue(_specialEnergy.GetValue());
            }
        }

        public float GetSpecialEnergyAmount()
        {
            return _specialEnergy.GetValue();
        }
    }
}