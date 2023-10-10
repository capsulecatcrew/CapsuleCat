using System;
using UnityEngine;

namespace Battle.Controllers.Player
{
    public class PlayerLaserController : MonoBehaviour
    {
        [SerializeField] private PlayerLaserPool laserPool;
        private Vector3 _forward1;
        private Vector3 _forward2;
        private Vector3 _origin1;
        private Vector3 _origin2;
        
        [Header("Audio")]
        [SerializeField] private PlayerSoundController playerSoundController;

        public delegate void ForwardChange(int playerNum, Vector3 forward);
        public event ForwardChange OnForwardChange;

        public delegate void OriginChange(int playerNum, Vector3 origin);
        public event OriginChange OnOriginChange;

        public void UpdateForward(int playerNum, Vector3 forward)
        {
            switch (playerNum)
            {
                case 1:
                    _forward1 = forward;
                    break;
                case 2:
                    _forward2 = forward;
                    break;
            }
            OnForwardChange?.Invoke(playerNum, forward);
        }

        public void UpdateOrigin(int playerNum, Vector3 origin)
        {
            switch (playerNum)
            {
                case 1:
                    _origin1 = origin;
                    break;
                case 2:
                    _origin2 = origin;
                    break;
            }
            OnOriginChange?.Invoke(playerNum, origin);
        }

        public PlayerLaser InitLaser(Firer firer, float damage)
        {
            var laser = laserPool.GetPooledObject();
            laser.InitPlayerSoundController(playerSoundController);
            switch (firer)
            {
                case Firer.Player1:
                    laser.Init(firer, damage, _origin1, _forward1);
                    break;
                case Firer.Player2:
                    laser.Init(firer, damage, _origin2, _forward2);
                    break;
                case Firer.Enemy:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(firer), firer, null);
            }
            return laser;
        }
    }
}