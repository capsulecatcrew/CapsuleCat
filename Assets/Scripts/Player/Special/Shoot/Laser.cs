using Battle;
using Battle.Controllers.Player;
using UnityEngine;

namespace Player.Special.Shoot
{
    public class Laser : SpecialMove
    {
        private const float Damage = 3f;
        private readonly Firer _firer;
        private PlayerLaser _laser;
        private PlayerLaserController _playerLaserController;
        
        private bool _isEnabled;

        public Laser(int playerNum) : base(playerNum, 3) // cost: 3
        {
            _firer = playerNum switch
            {
                1 => Firer.Player1,
                2 => Firer.Player2,
                _ => _firer
            };
        }

        public override void Enable() { }

        public override void Disable() { }

        public override void Start()
        {
            if (_isEnabled)
            {
                Stop();
                return;
            }
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            _laser = _playerLaserController.InitLaser(_firer, Damage);
            _playerLaserController.OnForwardChange += HandleLaserForwardChange;
            _playerLaserController.OnOriginChange += HandleLaserOriginChange;
            _laser.gameObject.SetActive(true);
            PlayerController.OnDeltaTimeUpdate += UpdateTimer;
            _isEnabled = true;
            PlayerSoundController.PlaySpecialEnabledSound();
            UpdateIcon();
        }

        public override void Stop(bool silent = false)
        {
            if (!_isEnabled) return;
            PlayerController.DisableShield(PlayerNum);
            _laser.gameObject.SetActive(false);
            _playerLaserController.OnForwardChange -= HandleLaserForwardChange;
            _playerLaserController.OnOriginChange -= HandleLaserOriginChange;
            PlayerController.OnDeltaTimeUpdate -= UpdateTimer;
            Disable();
            _isEnabled = false;
            PlayerSoundController.PlaySpecialDisabledSound();
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

        public void InitPlayerLaserController(PlayerLaserController playerLaserController)
        {
            _playerLaserController = playerLaserController;
        }

        private void HandleLaserForwardChange(int playerNum, Vector3 forward)
        {
            if (playerNum != PlayerNum) return;
            _laser.SetTargetForward(forward);
        }
        
        private void HandleLaserOriginChange(int playerNum, Vector3 origin)
        {
            if (playerNum != PlayerNum) return;
            _laser.SetLaserOrigin(origin);
        }
    }
}