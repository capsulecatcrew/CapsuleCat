using Player.Stats;

namespace Player.Special.Move
{
    public class AbsorbShield : SpecialMove
    {
        private const string Name = "Energy Shield";
        private const float Amount = 0.5f;
        private const string Description = "";
        
        private bool _isEnabled;
        
        public AbsorbShield(int playerNum) : base(playerNum, 3) { } // cost: 3

        public override void Enable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1ShieldHit += ApplyEffect;
                    return;
                case 2:
                    PlayerController.OnP2ShieldHit += ApplyEffect;
                    return;
            }
        }

        public override void Disable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1ShieldHit -= ApplyEffect;
                    return;
                case 2:
                    PlayerController.OnP2ShieldHit -= ApplyEffect;
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
            if (!PlayerController.EnableShield(PlayerNum)) return;
            Enable();
            PlayerController.OnDeltaTimeUpdate += UpdateTimer;
            _isEnabled = true;
            PlayerSoundController.PlaySpecialEnabledSound();
            UpdateIcon();
        }

        public override void Stop(bool silent = false)
        {
            if (!_isEnabled) return;
            PlayerController.DisableShield(PlayerNum);
            PlayerController.OnDeltaTimeUpdate -= UpdateTimer;
            Disable();
            _isEnabled = false;
            PlayerSoundController.PlaySpecialDisabledSound();
            UpdateIcon();
        }

        protected override void ApplyEffect(float amount)
        {
            if (!_isEnabled) return;
            if (amount >= 0) return;
            var absorbed = -amount;
            PlayerController.Heal(absorbed);
            PlayerController.AddEnergy(PlayerNum, absorbed * Amount);
        }
        
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

        private void UpdateTimer(float deltaTime)
        {
            if (!_isEnabled) return;
            var usage = deltaTime * Cost;
            if (!PlayerController.HasSpecial(PlayerNum, usage))
            {
                Stop();
                return;
            }
            PlayerController.UseSpecial(PlayerNum, usage);
        }
        
    }
}