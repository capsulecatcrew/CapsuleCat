using Battle.Hitboxes;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{    
    public class PlayerEnergyController : MonoBehaviour 
    {
        [SerializeField][Range(1, 2)] private int playerNum;
        [SerializeField] private PlayerEnergyController otherPlayerEnergy;
        [SerializeField] private Hitbox[] energyAbsorbers;
        private BattleStat _energy;
        [SerializeField] private WingGlow wingGlow;

        public delegate void EnergyChange(float amount);
        public event EnergyChange OnEnergyChange;

        public void InitBar(ProgressBar energyBar)
        {
            energyBar.SetValue(_energy.GetValue());
        }

        public void Awake()
        {
            _energy = PlayerStats.CreateBattleEnergyStat(playerNum);
        }
        
        public void OnEnable()
        {
            SubscribeToEvents();
        }

        public void OnDisable() 
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnHitBox += GainEnergy;
            }
            _energy.OnStatChange += HandleStatChange;
        }

        private void UnsubscribeFromEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnHitBox -= GainEnergy;
            }
            _energy.OnStatChange -= HandleStatChange;
        }

        private void GainEnergy(float amount, DamageType damageType)
        {
            if (damageType == DamageType.Special) return;
            var absorbedAmount = PlayerStats.ApplyEnergyAbsorbMultiplier(playerNum, amount);
            AddEnergy(absorbedAmount);
            wingGlow.TurnOnGlow();
            var sharedAmount = PlayerStats.ApplyEnergyShareMultiplier(playerNum, absorbedAmount);
            otherPlayerEnergy.AddEnergy(sharedAmount);
        }

        public void AddEnergy(float amount)
        {
            _energy.AddValue(amount);
        }
        
        internal void UseEnergy(float amount)
        {
            _energy.MinusValue(amount);
        }
        
        internal bool HasEnergy(float amount)
        {
            return _energy.CanMinusValue(amount);
        }

        private void HandleStatChange(float amount)
        {
            OnEnergyChange?.Invoke(amount);
        }
    }
}