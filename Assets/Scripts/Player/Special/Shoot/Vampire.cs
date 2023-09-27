using Enemy;
using UnityEngine;

namespace Player.Special.Shoot
{
    public class Vampire : SpecialMove
    {
        private const string Name = "Vampiric";

        private const float Multiplier = 0.15f;

        private new const float Cost = 5;
        private bool _isEnabled;
        private int _charges;
        
        

        public Vampire(int playerNum) : base(Name, playerNum, Cost)
        {

            EnemyController.OnEnemyMainDamaged += ApplyEffect;
        }

        private void AddCharge()
        {
            if (!_isEnabled) return;
            if (!PlayerController.GetPlayerSpecialEnergy(PlayerNum).HasSpecialEnergy(Cost))
            {
                Stop();
                return;
            }
            PlayerController.GetPlayerSpecialEnergy(PlayerNum).UseSpecialEnergy(Cost);
            ++_charges;
        }

        public override void Start()
        {
            if (_isEnabled)
            {
                Stop();
                return;
            }
            if (!PlayerController.GetPlayerSpecialEnergy(PlayerNum).HasSpecialEnergy(Cost)) return;
            _isEnabled = true;
            // PlayerController.OnPlayerShotFired += AddCharge;
            // PlayerController.OnEnemyHit += ApplyEffect;
        }

        public override void Stop()
        {
            _isEnabled = false;
            // PlayerController.OnPlayerShotFired -= AddCharge;
        }

        protected override void ApplyEffect(float amount)
        {
            if (_charges == 0) return;
            PlayerController.HealPlayer(amount * Multiplier);
            --_charges;
        }
    }
}