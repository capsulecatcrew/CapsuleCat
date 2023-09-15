using System.Collections;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField] private string[] tagsToHit;
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

    private bool _trackingTarget;
    private bool _nonStopTracking = false;
    
    public delegate void LaserHitUpdate(GameObject hitObject, float damage, bool ignoreIFrames);
    
    public event LaserHitUpdate OnLaserHitUpdate;


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
        var controller = GameObject.FindGameObjectWithTag("GameController");
        var battleManager = controller.GetComponent<BattleManager>();
        battleManager.RegisterLaser(this);
    }

    private void OnDisable()
    {
        hitbox.HitboxStay -= OnHitBoxStay;
        var controller = GameObject.FindGameObjectWithTag("GameController");
        var battleManager = controller.GetComponent<BattleManager>();
        battleManager.DeregisterLaser(this);
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
            if (!other.CompareTag(tag)) continue;
            OnLaserHitUpdate?.Invoke(other.gameObject, damage, false);
        }
    }
}
