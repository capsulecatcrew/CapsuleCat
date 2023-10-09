using Battle.Controllers;
using UnityEngine;

namespace Enemy
{
    public class EnemySoundController : SoundController
    {
        [Header("Attack Sounds")]
        [SerializeField] private AudioClip bulletSound;
        [SerializeField] private float bulletVolume = 1;
        [SerializeField] private AudioClip laserChargingSound;
        [SerializeField] private float laserChargingVolume = 1;
        [SerializeField] private AudioClip laserFiringSound;
        [SerializeField] private float laserFiringVolume = 1;
        
        public void PlayBulletSound()
        {
            audioSource.PlayOneShot(bulletSound, bulletVolume);
        }
        
        public void PlayLaserChargingSound(AudioSource localAudioSource)
        {
            localAudioSource.PlayOneShot(laserChargingSound, laserChargingVolume);
        }
        
        public void PlayLaserFiringSound(AudioSource localAudioSource)
        {
            localAudioSource.PlayOneShot(laserFiringSound, laserFiringVolume);
        }
    }
}