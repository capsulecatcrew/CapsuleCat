using Battle;
using Battle.Controllers.Player;
using Enemy;
using UnityEngine;

namespace Player.Special.Shoot
{
    public class Laser : SpecialMove
    {
        private float damage = 5;
        private HitboxTrigger hitbox;
        private readonly Firer _firer;
        private const DamageType DamageType = Battle.DamageType.Special;
        private static GenericObjectPool<PlayerLaser> laserPool = new ();
        private Transform target;

        private Animator animator;
        private static readonly int LockOnTrigger = Animator.StringToHash("Lock On");
        private static readonly int FireTrigger = Animator.StringToHash("Fire");
        private static readonly int FinishTrigger = Animator.StringToHash("Finish");

        private static EnemySoundController _enemySoundController;

        private float _chargingDuration = 2.5f;
        private float _lockOnDuration = 0.5f;
        private float _firingDuration = 2.0f;

        private bool _trackingTarget;

        public Laser(int playerNum) : base(playerNum, 3) // cost: 3
        {
            _firer = playerNum switch
            {
                1 => Firer.Player1,
                2 => Firer.Player2,
                _ => _firer
            };
        }

        public override void Enable()
        {
            throw new System.NotImplementedException();
        }

        public override void Disable()
        {
            throw new System.NotImplementedException();
        }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyEffect(float amount)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateIcon()
        {
            throw new System.NotImplementedException();
        }
    }
}