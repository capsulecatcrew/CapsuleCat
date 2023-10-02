using System.Collections;
using Battle;
using Battle.Hitboxes;
using Enemy;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage = 1;
    [SerializeField] private HitboxTrigger hitbox;
    [SerializeField] private DamageType damageType;
    [SerializeField] private Transform target;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    private static readonly int LockOnTrigger = Animator.StringToHash("Lock On");
    private static readonly int FireTrigger = Animator.StringToHash("Fire");
    private static readonly int FinishTrigger = Animator.StringToHash("Finish");

    [SerializeField] private AudioSource audioSource;
    
    private static EnemySoundController _enemySoundController;

    private float _chargingDuration = 2.5f;
    private float _lockOnDuration = 0.5f;
    private float _firingDuration = 2.0f;

    private bool _trackingTarget;

    public void Update()
    {
        if (_trackingTarget)
        {
            SetTargetPosition(target.position);
        }
    }

    public void OnEnable()
    {
        hitbox.HitboxStay += OnHitBoxStay;
        StartCoroutine(FireLaser());
    }

    public void OnDisable()
    {
        hitbox.HitboxStay -= OnHitBoxStay;
    }

    private IEnumerator FireLaser(bool trackAfterLockOn = false)
    {
        _enemySoundController.PlayLaserChargingSound(audioSource);
        yield return new WaitForSeconds(_chargingDuration);
        
        animator.SetTrigger(LockOnTrigger);
        _trackingTarget = trackAfterLockOn;
        yield return new WaitForSeconds(_lockOnDuration);
        
        animator.SetTrigger(FireTrigger);
        _enemySoundController.PlayLaserFiringSound(audioSource);
        yield return new WaitForSeconds(_firingDuration);
        
        animator.SetTrigger(FinishTrigger);
        yield return new WaitForSeconds(0.4f);
        
        gameObject.SetActive(false);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    
    public void SetTargetTracking(Transform target)
    {
        this.target = target;
        _trackingTarget = true;
    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        transform.rotation = Quaternion.LookRotation(targetPos - transform.position);
    }

    public void SetFiringTiming(float chargingDuration = 0, float lockOnDuration = 0, float firingDuration = 0)
    {
        if (chargingDuration > 0) _chargingDuration = chargingDuration;
        if (lockOnDuration > 0) _lockOnDuration = lockOnDuration;
        if (firingDuration > 0) _firingDuration = firingDuration;
    }

    public static void SetEnemySoundController(EnemySoundController enemySoundController)
    {
        _enemySoundController = enemySoundController;
    }

    public void DisableTargetTracking()
    {
        _trackingTarget = false;
    }
    
    private void OnHitBoxStay(Collider other)
    {
        var otherHitbox = other.gameObject.GetComponent<Hitbox>();
        if (otherHitbox == null) return;
        otherHitbox.Hit(Firer.Enemy, damage, damageType);
    }
}
