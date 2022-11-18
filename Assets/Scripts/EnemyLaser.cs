using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

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

    public float chargingDuration;
    public float lockOnDuration;
    public float firingDuration;

    public int damage;

    private bool _trackingTarget = false;
    
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
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
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

    IEnumerator FireLaser()
    {
        audioSource.PlayOneShot(chargingSound);
        yield return new WaitForSeconds(chargingDuration);
        animator.SetTrigger(LockOnTrigger);
        _trackingTarget = false;
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
                Damageable damageable = other.gameObject.GetComponent<Damageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(damage, false);
                }
            }
        }
    }
}
