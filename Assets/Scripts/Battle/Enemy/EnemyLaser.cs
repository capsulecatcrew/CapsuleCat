using Battle;
using Enemy;
using UnityEngine;

public class EnemyLaser : LaserController
{
    [SerializeField] private AudioSource audioSource;
    
    private static EnemySoundController _enemySoundController;

    public static void SetEnemySoundController(EnemySoundController enemySoundController)
    {
        _enemySoundController = enemySoundController;
    }

    protected override void PlayChargingSound()
    {
        _enemySoundController.PlayLaserChargingSound(audioSource);
    }
    
    protected override void PlayFiringSound()
    {
        _enemySoundController.PlayLaserFiringSound(audioSource);
    }
}
