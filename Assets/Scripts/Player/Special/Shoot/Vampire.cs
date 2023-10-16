using Player.Stats;

namespace Player.Special.Shoot
{
    public class Vampire : SpecialMove
    {
        private const string Name = "Vampiric";
        private const float Amount = 0.15f;
        private const string Description = "";
        
        private bool _isEnabled;

        public Vampire(int playerNum) : base(playerNum, 5) { } // cost: 5

        public override void Enable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1BulletShoot += HandleBulletShoot;
                    return;
                case 2:
                    PlayerController.OnP2BulletShoot += HandleBulletShoot;
                    return;
            }
        }
        
        public override void Disable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1BulletShoot -= HandleBulletShoot;
                    return;
                case 2:
                    PlayerController.OnP2BulletShoot -= HandleBulletShoot;
                    return;
            }
        }

        public override void Start()
        {
            if (_isEnabled)
            {
                Stop();
                return;
            }
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            Enable();
            _isEnabled = true;
            PlayerSoundController.PlaySpecialEnabledSound();
            UpdateIcon();
        }

        public override void Stop(bool silent = false)
        {
            if (!_isEnabled) return;
            if (!silent) PlayerSoundController.PlaySpecialDisabledSound();
            Disable();
            _isEnabled = false;
            UpdateIcon();
        }

        protected override void ApplyEffect(float amount) { }
        
        protected override void UpdateIcon()
        {
            switch (_isEnabled)
            {
                case true:
                    SpecialIcon.StartSpecial(this);
                    return;
                case false:
                    SpecialIcon.StopSpecial(this);
                    return;
            }
        }

        private void ApplyEffect(Bullet bullet, float amount)
        {
            PlayerController.Heal(amount * Amount);
            bullet.OnBulletHitUpdate -= ApplyEffect;
        }

        private void HandleBulletShoot(Bullet bullet)
        {
            if (!_isEnabled) return;
            if (!PlayerController.HasSpecial(PlayerNum, Cost))
            {
                Stop();
                return;
            }
            PlayerController.UseSpecial(PlayerNum, Cost);
            bullet.OnBulletHitUpdate += ApplyEffect;
        }

    }
}