using Player.Stats.Persistent;
using UnityEngine;
using Battle.Hitboxes;
using UnityEngine.Serialization;

namespace Battle.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Hitbox playerBody;
        [SerializeField] private PlayerEnergyController p1Energy, p2Energy;
        [SerializeField] private PlayerSpecialController p1SpecialEnergy, p2SpecialEnergy;
        [SerializeField] private Hitbox p1ManaShield;
        [SerializeField] private Hitbox p2ManaShield;

        private BattleStat _playerHealth;
        public delegate void StatChange(float change);
        public event StatChange OnHealthChange;
        public event StatChange OnP1EnergyChange, OnP2EnergyChange;
        public event StatChange OnP1SpecialChange, OnP2SpecialChange;
        
        public delegate void NoParams();
        public event NoParams OnPlayerDeath;

        void Awake()
        {
            InitializeBattleStats();
        }

        void OnEnable()
        {
            SubscribeToAllEvents();
        }

        void OnDisable()
        {
            UnsubscribeFromAllEvents();
        }

        private void InitializeBattleStats()
        {
            _playerHealth = PlayerStats.CreateBattleHealthStat();
        }

        private void SubscribeToAllEvents()
        {
            playerBody.OnDamaged += HandlePlayerDamaged;
            p1Energy.OnEnergyUpdate += HandleP1EnergyChange;
            p2Energy.OnEnergyUpdate += HandleP2EnergyChange;
            p1SpecialEnergy.OnSpecialChange += HandleP1SpecialChange;
            p2SpecialEnergy.OnSpecialChange += HandleP2SpecialChange;
        }

        private void UnsubscribeFromAllEvents()
        {
            playerBody.OnDamaged -= HandlePlayerDamaged;
            p1Energy.OnEnergyUpdate -= HandleP1EnergyChange;
            p2Energy.OnEnergyUpdate -= HandleP2EnergyChange;
            p1SpecialEnergy.OnSpecialChange -= HandleP1SpecialChange;
            p2SpecialEnergy.OnSpecialChange -= HandleP2SpecialChange;
        }

        private void HandlePlayerDamaged(float dmg, DamageType damageType)
        {
            // special type attacks do 2x damage
            float netDmg = damageType == DamageType.Normal ? dmg : dmg * 2;
            _playerHealth.MinusValue(netDmg);
            OnHealthChange?.Invoke(-netDmg);
            CheckForPlayerDeath();
        }

        public void HealPlayer(float amount)
        {
            _playerHealth.AddValue(amount);
            OnHealthChange?.Invoke(amount);
        }

        private void HandleEnergyChange(int playerNum, float amount)
        {
            switch(playerNum)
            {
                case 1:
                    OnP1EnergyChange?.Invoke(amount);
                    break;
                case 2:
                    OnP2EnergyChange?.Invoke(amount);
                    break;
            }
        }

        private void HandleP1EnergyChange(float amount)
        {
            HandleEnergyChange(1, amount);
        }

        private void HandleP2EnergyChange(float amount)
        {
            HandleEnergyChange(2, amount);
        }

        private void HandleSpecialChange(int playerNum, float amount)
        {
            switch(playerNum)
            {
                case 1:
                    OnP1SpecialChange?.Invoke(amount);
                    break;
                case 2:
                    OnP2SpecialChange?.Invoke(amount);
                    break;
            }
        }

        private void HandleP1SpecialChange(float amount)
        {
            HandleSpecialChange(1, amount);
        }
        private void HandleP2SpecialChange(float amount)
        {
            HandleSpecialChange(2, amount);
        }

        private void CheckForPlayerDeath()
        {
            if (_playerHealth.GetValue() <= 0) HandlePlayerDeath();
        }

        private void HandlePlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }

        public float GetCurrentHealth()
        {
            return _playerHealth.GetValue();
        }

        public PlayerEnergyController GetPlayerEnergy(int playerNum)
        {
            return playerNum switch
            {
                1 => p1Energy,
                2 => p2Energy,
                _ => p2Energy
            };
        }
        public PlayerSpecialController GetPlayerSpecialEnergy(int playerNum)
        {
            return playerNum switch
            {
                1 => p1SpecialEnergy,
                2 => p2SpecialEnergy,
                _ => p2SpecialEnergy
            };
        }
    }
}