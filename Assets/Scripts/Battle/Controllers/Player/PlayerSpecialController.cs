using Battle.Hitboxes;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{    
    public class PlayerSpecialController : MonoBehaviour 
    {
        [SerializeField][Range(1, 2)] private int playerNum = 1;
        [SerializeField] private Hitbox[] energyAbsorbers;
        [SerializeField] private Hitbox playerBody;
        private BattleStat _special;

        public delegate void SpecialChange(float change);
        public event SpecialChange OnSpecialChange;
        
        public void InitBar(ProgressBar specialBar)
        {
            specialBar.SetValue(_special.GetValue());
        }
        
        public void Awake()
        {
            _special = PlayerStats.CreateBattleSpecialStat(playerNum);
        }
        
        public void OnEnable() 
        {
            PlayerStats.EnableSpecialMoves();
            SubscribeToEvents();
        }

        public void OnDisable() 
        {
            PlayerStats.DisableSpecialMoves();
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnHitBox += GainAbsorbed;
            }
            _special.OnStatChange += HandleStatChange;
            playerBody.OnHitBox += GainDamaged;
        }

        private void UnsubscribeFromEvents()
        {
            foreach (var absorber in energyAbsorbers)
            {
                absorber.OnHitBox -= GainAbsorbed;
            }
            _special.OnStatChange -= HandleStatChange;
            playerBody.OnHitBox -= GainDamaged;
        }

        private void GainAbsorbed(float amount)
        {
            AddSpecial(PlayerStats.ApplySpecialAbsorbMultipler(playerNum, amount));
        }

        private void GainDamaged(float amount)
        {
            AddSpecial(PlayerStats.ApplySpecialDamagedMultipler(playerNum, amount));
        }

        private void AddSpecial(float amount)
        {
            _special.AddValue(amount);
        }

        internal bool HasSpecial(float amount)
        {
            return _special.CanMinusValue(amount);
        }
        
        internal void UseSpecial(float amount)
        {
            _special.MinusValue(amount);
        }

        private void HandleStatChange(float amount)
        {
            OnSpecialChange?.Invoke(amount);
        }
    }
}