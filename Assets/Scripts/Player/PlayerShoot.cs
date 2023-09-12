using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    public GameObject[] weapons;

    public PlayerEnergy playerEnergy;

    [SerializeField] private ObjectPool basicBulletPool;
    [SerializeField] private ObjectPool lowBulletPool;
    [SerializeField] private ObjectPool heavyBulletPool;
    [SerializeField] private ScreenShaker screenShaker;
    [SerializeField] private PlayerMovement _playerMovement;

    public AudioClip shootingAudio;

    public AudioSource audioSource;

    public string[] tagsToHit;

    public Transform target;

    public float aimSpeed = 20;
    public float aimXLimit = 35;
    public float aimYLimit = 45;

    // General bullet info
    public float bulletCooldown = 0.5f;
    public Transform[] shootingOrigins;

    // Basic bullet fields
    public int basicDamage = 2;
    public float basicSpeed = 20;
    public float basicTravelDist = 50;
    public float basicEnergyCost = 1;
    private float _cooldownTime;

    // Weak bullet fields
    [SerializeField] private float weakDamageMultiplier = 0.7f;
    [SerializeField] private float weakSpeedMultiplier = 0.5f;
    [SerializeField] private float weakCooldownMultiplier = 3.0f;

    // Heavy bullet fields
    [SerializeField] private float heavyMinCharge = 1.5f;
    [SerializeField] private float heavyMaxCharge = 4.0f;
    [SerializeField] private float heavyDamageMultiplier = 2.0f;
    [SerializeField] private float heavySpeedMultiplier = 0.2f;
    [SerializeField] private float heavyCooldownMultiplier = 0.5f;
    [SerializeField] private float heavyEnergyCostMultiplier = 3.0f;
    [SerializeField] private float heavyScreenShakeMultiplier = 2f;
    [SerializeField] private int heavyScreenShakeMaxAmount = 4;
    [SerializeField] private float heavySizeMultiplier = 0.35f;
    private bool _isHeavyCharging;
    private float _heavyChargeTime;
    private GameObject _heavyBullet;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        _cooldownTime = 0;
        bulletCooldown = PlayerStats.FiringRate.GetCurrentValue();
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        foreach (Transform shootingOrigin in shootingOrigins)
        {
            Vector3 position = shootingOrigin.position;
            Debug.DrawLine(position, position + shootingOrigin.forward, Color.cyan);
        }
    }

    /// <summary>
    /// Shifts gun aim by given amount.
    /// </summary>
    /// <param name="dir">Vector2 by which to shift guns aim by</param>
    public void MoveGunsBy(Vector2 dir)
    {
        foreach (GameObject gun in weapons)
        {
            gun.transform.Rotate(new Vector3(0, dir.x * Time.deltaTime * aimSpeed, 0), Space.World);
            gun.transform.Rotate(new Vector3(dir.y * Time.deltaTime * aimSpeed, 0, 0), Space.Self);

            // optimise this chunk of code later
            float xRot = gun.transform.rotation.eulerAngles.x;
            float yRot = gun.transform.localRotation.eulerAngles.y;
            if (xRot <= 180 && xRot > aimXLimit)
            {
                gun.transform.rotation = Quaternion.Euler(aimXLimit, gun.transform.rotation.eulerAngles.y, 0);
            }
            else if (xRot > 180 && xRot < 360 - aimXLimit)
            {
                gun.transform.rotation = Quaternion.Euler(360 - aimXLimit, gun.transform.rotation.eulerAngles.y, 0);
            }

            if (yRot <= 180 && yRot > aimYLimit)
            {
                gun.transform.localRotation = Quaternion.Euler(gun.transform.rotation.eulerAngles.x, aimYLimit, 0);
            }
            else if (yRot > 180 && yRot < 360 - aimYLimit)
            {
                gun.transform.localRotation =
                    Quaternion.Euler(gun.transform.rotation.eulerAngles.x, 360 - aimYLimit, 0);
            }
        }
    }

    /// <summary>
    /// Checks if firing cooldown is over.
    /// </summary>
    private bool IsCooldownOver()
    {
        return _cooldownTime <= float.Epsilon;
    }

    /// <summary>
    /// Checks if a player can fire a weak bullet.
    /// </summary>
    private bool CanShootWeak()
    {
        return IsCooldownOver() && !_isHeavyCharging;
    }

    /// <summary>
    /// Checks if a player can fire a basic bullet.
    /// </summary>
    private bool CanShootBasic()
    {
        return IsCooldownOver() && playerEnergy.HasEnergy(basicEnergyCost) && !_isHeavyCharging;
    }

    /// <summary>
    /// Checks if a player can fire a heavy bullet.
    /// </summary>
    /// <param name="chargeTime">Fired heavy bullet's charge time.</param>
    /// <param name="energyCost">Fired heavy bullet's energy cost.</param>
    /// <returns></returns>
    private bool CanShootHeavy(double chargeTime, float energyCost)
    {
        return IsCooldownOver() && chargeTime >= heavyMinCharge && playerEnergy.HasEnergy(energyCost);
    }

    public void ChargeHeavyBullet()
    {
        if (!IsCooldownOver()) return;
        _isHeavyCharging = true;
        screenShaker.ChargeShake(heavyMinCharge, heavyScreenShakeMultiplier, heavyScreenShakeMaxAmount);
        TransformHeavyBullet();
    }

    /// <summary>
    /// Fires off heavy bullet.
    /// </summary>
    /// <param name="chargeTime">Fired heavy bullet's charge time.</param>
    public void ShootHeavyBullet(float chargeTime)
    {
        ResetHeavyCharge();
        screenShaker.EndShake();
        float clampedTime = Math.Clamp(chargeTime, 0, heavyMaxCharge);
        float chargePercent = clampedTime / heavyMaxCharge;
        float energyCost = basicEnergyCost * heavyEnergyCostMultiplier * chargePercent;

        if (!CanShootHeavy(clampedTime, energyCost))
        {
            DestroyHeavyBullet();
            ShootBasicBullets();
            return;
        }

        ReleaseHeavyBullet(chargePercent);

        playerEnergy.AddAmount(-energyCost);
        _cooldownTime = bulletCooldown + clampedTime / heavyCooldownMultiplier;

        PlayHeavyBulletAudio();
    }

    public void ShootBasicBullets()
    {
        if (!CanShootBasic())
        {
            ShootWeakBullets();
            return;
        }

        playerEnergy.AddAmount(-basicEnergyCost);

        TransformBullets(basicBulletPool, basicDamage, basicSpeed);

        _cooldownTime = bulletCooldown;

        PlayBulletAudio();
    }

    private void ShootWeakBullets()
    {
        if (!CanShootWeak()) return;

        int damage = (int)Math.Round(basicDamage * weakDamageMultiplier);
        float speed = basicSpeed * weakSpeedMultiplier;
        TransformBullets(lowBulletPool, damage, speed);

        _cooldownTime = bulletCooldown * weakCooldownMultiplier;

        PlayBulletAudio();
    }

    private void TransformBullets(ObjectPool bulletPool, int damage, float speed)
    {
        foreach (Transform shootingOrigin in shootingOrigins)
        {
            GameObject bullet = bulletPool.GetPooledObject();

            bullet.transform.position = shootingOrigin.position;
            Vector3 direction = shootingOrigin.forward;

            bullet.GetComponent<Bullet>().Init(damage, direction, speed, basicTravelDist, tagsToHit);
            bullet.SetActive(true);
        }
    }

    /// <summary>
    /// Spawn heavy bullet for charging.
    /// </summary>
    private void TransformHeavyBullet()
    {
        _heavyBullet = heavyBulletPool.GetPooledObject();

        _heavyBullet.transform.position = CalculateHeavyPosition();

        _heavyBullet.GetComponent<Bullet>().Init(1, CalculateHeavyForward(), 0, basicTravelDist, tagsToHit);
        _heavyBullet.SetActive(true);
    }

    private Vector3 CalculateHeavyPosition()
    {
        Vector3 positionL = shootingOrigins[0].position;
        Vector3 positionR = shootingOrigins[1].position;
        return positionL + (positionR - positionL) / 2;
    }

    private Vector3 CalculateHeavyForward()
    {
        Vector3 forwardL = shootingOrigins[0].forward;
        Vector3 forwardR = shootingOrigins[1].forward;
        return forwardL + (forwardR - forwardL) / 2;
    }

    private void ReleaseHeavyBullet(float chargePercent)
    {
        int damage = (int)Math.Floor(basicDamage * heavyDamageMultiplier * chargePercent);
        float speed = basicSpeed * heavySpeedMultiplier / chargePercent;

        _heavyBullet.GetComponent<Bullet>().Fire(CalculateHeavyForward(), damage, speed);
    }

    private void DestroyHeavyBullet()
    {
        _heavyBullet.SetActive(false);
    }

    private void PlayBulletAudio()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(shootingAudio);
    }
    
    private void PlayHeavyBulletAudio()
    {
        audioSource.pitch = Random.Range(0.3f, 0.5f);
        audioSource.PlayOneShot(shootingAudio);
    }

    private void UpdateCooldown(float deltaTime)
    {
        if (IsCooldownOver()) return;
        _cooldownTime -= deltaTime;
    }

    private void UpdateHeavyCharge(float deltaTime)
    {
        if (_isHeavyCharging)
        {
            _heavyChargeTime += deltaTime;
            float clampedTime = Math.Clamp(_heavyChargeTime, 0, heavyMaxCharge);
            var scalar = clampedTime * heavySizeMultiplier;
            Vector3 scale = new Vector3(scalar, scalar, scalar);
            _heavyBullet.transform.localScale = scale;
            _heavyBullet.transform.position = CalculateHeavyPosition();
            print(CalculateHeavyPosition());
            float slowMultiplier = 1.5f - clampedTime / heavyMaxCharge;
            _playerMovement.slowSpeed(slowMultiplier);
        }
    }

    private void ResetHeavyCharge()
    {
        _isHeavyCharging = false;
        _heavyChargeTime = 0;
        _playerMovement.resetMaxSpeed();
    }

    public bool IsHeavyCharging()
    {
        return _isHeavyCharging;
    }

    void Update()
    {
        UpdateCooldown(Time.deltaTime);
        UpdateHeavyCharge(Time.deltaTime);
    }
}