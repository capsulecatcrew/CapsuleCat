using UnityEngine;

namespace Battle.Controllers.Player
{
    public class PlayerLaser : LaserController
    {
        [SerializeField] private float playerNum;
        private PlayerSoundController _playerSoundController;

        protected override void PlayChargingSound()
        {
            _playerSoundController.PlayLaserChargingSound();
        }
    
        protected override void PlayFiringSound()
        {
            _playerSoundController.PlayLaserFiringSound();
        }
    }
}