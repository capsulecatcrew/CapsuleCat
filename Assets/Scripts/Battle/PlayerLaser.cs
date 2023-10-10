namespace Battle.Controllers.Player
{
    public class PlayerLaser : LaserController
    {
        private PlayerSoundController _playerSoundController;

        protected override void PlayChargingSound()
        {
            _playerSoundController.PlayLaserChargingSound();
        }
    
        protected override void PlayFiringSound()
        {
            _playerSoundController.PlayLaserFiringSound();
        }

        public void InitPlayerSoundController(PlayerSoundController playerSoundController)
        {
            _playerSoundController = playerSoundController;
        }
    }
}