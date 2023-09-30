namespace Player.Special.Shoot
{
    public class Vampire : SpecialMove
    {
        private const string Name = "Vampiric";

        private const float Multiplier = 0.15f;

        private new const float Cost = 5;
        private bool _isEnabled;

        public Vampire(int playerNum) : base(Name, playerNum, Cost)
        {
            switch (playerNum)
            {
                case 1:
                    PlayerController.OnP1BulletShoot += HandleBulletShoot;
                    return;
                case 2:
                    PlayerController.OnP2BulletShoot += HandleBulletShoot;
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
            _isEnabled = true;
        }

        public override void Stop()
        {
            _isEnabled = false;
        }

        protected override void ApplyEffect(float amount)
        {
            PlayerController.Heal(amount * Multiplier);
        }

        private void HandleBulletShoot(Bullet bullet)
        {
            if (!_isEnabled) return;
            bullet.OnBulletHitUpdate += ApplyEffect;
        }
    }
}