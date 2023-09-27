using System;
using System.Collections;
using Battle;
using Battle.Hitboxes;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public HitboxTrigger hitbox;
    [SerializeField] private DamageType _damageType = DamageType.Normal;
    [SerializeField] private Transform target;
    
    public Animator animator;
    private static readonly int LockOnTrigger = Animator.StringToHash("Lock On");
    private static readonly int FireTrigger = Animator.StringToHash("Fire");
    private static readonly int FinishTrigger = Animator.StringToHash("Finish");

    public AudioSource audioSource;
    
    public AudioClip chargingSound;
    public AudioClip firingSound;

    private float _chargingDuration = 2.5f;
    private float _lockOnDuration = 0.5f;
    private float _firingDuration = 2.0f;

    private int _damage = 1;

    private bool _trackingTarget;
    private bool _nonStopTracking = false;
    
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

    IEnumerator FireLaser(bool trackAfterLockOn = false)
    {
        audioSource.PlayOneShot(chargingSound);
        yield return new WaitForSeconds(_chargingDuration);
        
        animator.SetTrigger(LockOnTrigger);
        _trackingTarget = trackAfterLockOn;
        yield return new WaitForSeconds(_lockOnDuration);
        
        animator.SetTrigger(FireTrigger);
        audioSource.PlayOneShot(firingSound);
        yield return new WaitForSeconds(_firingDuration);
        
        animator.SetTrigger(FinishTrigger);
        yield return new WaitForSeconds(0.4f);
        
        gameObject.SetActive(false);
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
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
        if (chargingDuration > 0) this._chargingDuration = chargingDuration;
        if (lockOnDuration > 0) this._lockOnDuration = lockOnDuration;
        if (firingDuration > 0) this._firingDuration = firingDuration;
    }

    public void DisableTargetTracking()
    {
        _trackingTarget = false;
    }
    
    private void OnHitBoxStay(Collider other)
    {
        var otherHitbox = other.gameObject.GetComponent<Hitbox>();
        if (otherHitbox == null) return;
        otherHitbox.Hit(Firer.Enemy, _damage, false, _damageType);
    }
}
