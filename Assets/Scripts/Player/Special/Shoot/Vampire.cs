namespace Player.Special.Shoot
{
    public class Vampire : SpecialMove
    {
        private const string Name = "Vampiric";

        private const float Multiplier = 0.3f;

        private new const float Cost = 5;
        private bool _isEnabled;
        private int _charges;

        public Vampire(int playerNum) : base(Name, playerNum, Cost)
        {
            BattleManager.OnEnemyHit += ApplyEffect;
        }

        private void AddCharge()
        {
            if (!_isEnabled) return;
            if (!BattleManager.HasSpecial(PlayerNum, Cost))
            {
                Stop();
                return;
            }
            BattleManager.UseSpecial(PlayerNum, Cost);
            ++_charges;
        }

        public override void Start()
        {
            if (_isEnabled)
            {
                Stop();
                return;
            }
            if (!BattleManager.HasSpecial(PlayerNum, Cost)) return;
            _isEnabled = true;
            BattleManager.OnPlayerShotFired += AddCharge;
            BattleManager.OnEnemyHit += ApplyEffect;
        }

        public override void Stop()
        {
            _isEnabled = false;
            BattleManager.OnPlayerShotFired -= AddCharge;
        }

        protected override void ApplyEffect(float amount)
        {
            if (_charges == 0) return;
            BattleManager.HealPlayer(amount * Multiplier);
            --_charges;
        }
    }
}