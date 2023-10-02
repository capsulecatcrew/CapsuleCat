using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle.Controllers.Player
{
    public class PlayerSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        [Header("Attack Sounds")]
        [SerializeField] private AudioClip cantShootSound;
        [SerializeField] private float cantShootVolume = 1;
        [SerializeField] private AudioClip basicShotSound;
        [SerializeField] private float basicShotVolume = 1;
        [SerializeField] private AudioClip weakShotSound;
        [SerializeField] private float weakShotVolume = 1;
        [SerializeField] private AudioClip heavyShotChargingSound;
        [SerializeField] private float heavyShotChargingVolume = 1;
        [SerializeField] private AudioClip heavyShotReadySound;
        [SerializeField] private float heavyShotReadyVolume = 1;
        [SerializeField] private AudioClip heavyShotReleaseSound;
        [SerializeField] private float heavyShotReleaseVolume = 1;
        [SerializeField] private AudioClip specialShotEnableSound;
        [SerializeField] private float specialShotEnableVolume = 1;
        [SerializeField] private AudioClip specialShotDisableSound;
        [SerializeField] private float specialShotDisableVolume = 1;
        private float _heavySoundCooldown;
        private const float HeavyShotChargingSoundLength = 0.05f;
        private bool _hasP1PlayedReadySound;
        private bool _hasP2PlayedReadySound;
        
        [Header("Movement Sounds")]
        [SerializeField] private AudioClip p1JumpSound;
        [SerializeField] private float p1JumpVolume = 1;
        [SerializeField] private AudioClip p2JumpSound;
        [SerializeField] private float p2JumpVolume = 1;
        [SerializeField] private AudioClip dashSound;
        [SerializeField] private float dashVolume = 1;

        public void Update()
        {
            UpdateHeavySoundCooldown(Time.deltaTime);
        }
        
        private void UpdateHeavySoundCooldown(float deltaTime)
        {
            if (_heavySoundCooldown <= 0) return;
            _heavySoundCooldown -= deltaTime;
        }

        public void PlayCantShootSound()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(cantShootSound, cantShootVolume);
        }

        public void PlayBasicBulletShotSound()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(basicShotSound, basicShotVolume);
        }

        public void PlayWeakBulletShotSound()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(weakShotSound, weakShotVolume);
        }

        public void PlayHeavyBulletChargingSound(float chargePercent)
        {
            if (_heavySoundCooldown > 0) return;
            audioSource.pitch = chargePercent;
            audioSource.volume = chargePercent;
            audioSource.PlayOneShot(heavyShotChargingSound, heavyShotChargingVolume);
            _heavySoundCooldown = HeavyShotChargingSoundLength;
        }

        public void PlayHeavyBulletReadySound(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    if (_hasP1PlayedReadySound) return;
                    break;
                case 2:
                    if (_hasP2PlayedReadySound) return;
                    break;
            }
            audioSource.PlayOneShot(heavyShotReadySound, heavyShotReadyVolume);
            switch (playerNum)
            {
                case 1:
                    _hasP1PlayedReadySound = true;
                    return;
                case 2:
                    _hasP2PlayedReadySound = true;
                    return;
            }
        }
        
        public void ResetHeavyBulletReadySound(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    _hasP1PlayedReadySound = false;
                    return;
                case 2:
                    _hasP2PlayedReadySound = false;
                    return;
            }
        }
        
        public void PlayHeavyBulletReleaseSound()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(heavyShotReleaseSound, heavyShotReleaseVolume);
        }
        
        // TODO: @yukun please help me call these :pleading:
        public void PlaySpecialEnabledSound()
        {
            audioSource.PlayOneShot(specialShotEnableSound, specialShotEnableVolume);
        }
        
        public void PlaySpecialDisabledSound()
        {
            audioSource.PlayOneShot(specialShotDisableSound, specialShotDisableVolume);
        }

        public void PlayJumpSound(int playerNum)
        {
            switch (playerNum)
            {
                case 1:
                    audioSource.PlayOneShot(p1JumpSound, p1JumpVolume);
                    return;
                case 2:
                    audioSource.PlayOneShot(p2JumpSound, p2JumpVolume);
                    return;
            }
        }

        public void PlayDashSound()
        {
            audioSource.PlayOneShot(dashSound, dashVolume);
        }
    }
}