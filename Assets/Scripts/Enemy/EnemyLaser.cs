using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public string[] tagsToHit;
    public HitboxTrigger hitbox;
    public Transform target;
    
    public Animator animator;
    private static readonly int LockOnTrigger = Animator.StringToHash("Lock On");
    private static readonly int FireTrigger = Animator.StringToHash("Fire");
    private static readonly int FinishTrigger = Animator.StringToHash("Finish");

    public AudioSource audioSource;
    
    public AudioClip chargingSound;
    public AudioClip firingSound;

    public float chargingDuration = 2.5f;
    public float lockOnDuration = 0.5f;
    public float firingDuration = 2.0f;

    public int damage = 1;

    private bool _trackingTarget = false;
    private bool _nonStopTracking = false;
    
    public delegate void Trigger();

    public event Trigger OnFinish;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_trackingTarget)
        {
            SetTargetPosition(target.position);
        }
    }

    private void OnEnable()
    {
        hitbox.HitboxStay += OnHitBoxStay;
        StartCoroutine(FireLaser());
    }

    private void OnDisable()
    {
        hitbox.HitboxStay -= OnHitBoxStay;
    }

    IEnumerator FireLaser(bool trackAfterLockOn = false)
    {
        audioSource.PlayOneShot(chargingSound);
        yield return new WaitForSeconds(chargingDuration);
        animator.SetTrigger(LockOnTrigger);
        _trackingTarget = trackAfterLockOn;
        yield return new WaitForSeconds(lockOnDuration);
        animator.SetTrigger(FireTrigger);
        audioSource.PlayOneShot(firingSound);
        yield return new WaitForSeconds(firingDuration);
        animator.SetTrigger(FinishTrigger);
        yield return new WaitForSeconds(0.4f);
        OnFinish?.Invoke();
        gameObject.SetActive(false);
    }

    public void SetDamage(int damage)
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
        if (chargingDuration > 0) this.chargingDuration = chargingDuration;
        if (lockOnDuration > 0) this.lockOnDuration = lockOnDuration;
        if (firingDuration > 0) this.firingDuration = firingDuration;
    }

    public void DisableTargetTracking()
    {
        _trackingTarget = false;
    }
    
    private void OnHitBoxStay(Collider other)
    {
        foreach (string tag in tagsToHit)
        {
            if (other.CompareTag(tag))
            {
                Damageable damageable = other.gameObject.GetComponent(typeof(Damageable)) as Damageable;

                if (damageable != null)
                {
                    damageable.TakeDamage(damage, false, Damageable.DamageType.Laser);
                }
            }
        }
    }
}
