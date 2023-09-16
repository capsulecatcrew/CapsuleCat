using System;
using Battle;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour, IPlayerSpecialUser
{
    [Header("Trackers")]
    [SerializeField] [Range(1, 2)] private int playerNum;
    public GameObject[] weapons;
    public Transform[] shootingOrigins;
    [SerializeField] private ScreenShaker screenShaker;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private BattleManager battleManager;

    // General bullet info
    private float _cooldownTime;
    private const float Speed = 20;

    [Header("Basic Bullets")]
    private float _damage;
    private const float EnergyCost = 1;
    [SerializeField] private float basicCooldownTime = 0.5f;
    [SerializeField] private ObjectPool basicBulletPool;
    
    [Header("Weak Bullets")]
    [SerializeField] private float weakDamageMultiplier = 0.7f;
    [SerializeField] private float weakSpeedMultiplier = 0.5f;
    [SerializeField] private float weakCooldownMultiplier = 3.0f;
    [SerializeField] private ObjectPool lowBulletPool;
    
    [Header("Heavy Bullets")]
    [SerializeField] private float heavyMinCharge = 1.5f;
    [SerializeField] private float heavyMaxCharge = 3.0f;
    [SerializeField] private float heavyDamageMultiplier = 6.0f;
    [SerializeField] private float heavySpeedMultiplier = 0.2f;
    [SerializeField] private float heavyCooldownMultiplier = 0.5f;
    [SerializeField] private float heavyEnergyCostMultiplier = 3.0f;
    [SerializeField] private float heavyScreenShakeMultiplier = 2f;
    [SerializeField] private int heavyScreenShakeMaxAmount = 4;
    [SerializeField] private float heavySizeMultiplier = 0.35f;
    private bool _isHeavyCharging;
    private float _heavyChargeTime;
    private GameObject _heavyObject;
    private Bullet _heavyBullet;
    [SerializeField] private ObjectPool heavyBulletPool;

    [Header("Special")]
    private PlayerSpecialMove _specialMove;

    [Header("Aiming")]
    [SerializeField] private float aimSpeed = 20;
    [SerializeField] private float aimXLimit = 35;
    [SerializeField] private float aimYLimit = 45;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootingAudio;

    [Header("Particles")]
    [SerializeField] private ParticleSystem particlesL;
    [SerializeField] private ParticleSystem particlesR;

    public void Start()
    {
        _damage = PlayerStats.GetDamage(playerNum);
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        foreach (var shootingOrigin in shootingOrigins)
        {
            var position = shootingOrigin.position;
            Debug.DrawLine(position, position + shootingOrigin.forward, Color.cyan);
        }
    }

    /// <summary>
    /// Shifts gun aim by given amount.
    /// </summary>
    /// <param name="dir">Vector2 by which to shift guns aim by</param>
    public void MoveGunsBy(Vector2 dir)
    {
        foreach (var gun in weapons)
        {
            gun.transform.Rotate(new Vector3(0, dir.x * Time.deltaTime * aimSpeed, 0), Space.World);
            gun.transform.Rotate(new Vector3(dir.y * Time.deltaTime * aimSpeed, 0, 0), Space.Self);

            // optimise this chunk of code later
            var xRot = gun.transform.rotation.eulerAngles.x;
            var yRot = gun.transform.localRotation.eulerAngles.y;
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
    /// Checks if player can switch control modes.
    /// Returns false if player is doing any of the below:
    /// - charging a heavy attack
    /// - using a continuous drain special attack.
    /// </summary>
    public bool CanSwitchControlMode()
    {
        return IsHeavyCharging();
    }

    private bool IsHeavyCharging()
    {
        return _isHeavyCharging;
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
        return IsCooldownOver() && !_isHeavyCharging && battleManager.HasEnergy(playerNum, EnergyCost);
    }

    /// <summary>
    /// Checks if a player can fire a heavy bullet.
    /// </summary>
    /// <param name="chargeTime">Fired heavy bullet's charge time.</param>
    /// <param name="energyCost">Fired heavy bullet's energy cost.</param>
    private bool CanShootHeavy(double chargeTime, float energyCost)
    {
        return IsCooldownOver() && chargeTime >= heavyMinCharge && battleManager.HasEnergy(playerNum, energyCost);
    }

    /// <summary>
    /// Starts charging up a heavy bullet.
    /// </summary>
    public void ChargeHeavyBullet()
    {
        if (!IsCooldownOver()) return;
        _isHeavyCharging = true;
        screenShaker.ChargedShake(heavyMinCharge, heavyScreenShakeMultiplier, heavyScreenShakeMaxAmount);
        TransformHeavyBullet();
    }

    /// <summary>
    /// Fires off heavy bullet.
    /// </summary>
    /// <param name="chargeTime">Fired heavy bullet's charge time.</param>
    public void ShootHeavyBullet(float chargeTime)
    {
        if (!_isHeavyCharging)
        {
            PlayCantShootParticles(true);
            return;
        }
        ResetHeavyCharge();
        screenShaker.EndShake();
        var clampedTime = Math.Clamp(chargeTime, 0, heavyMaxCharge);
        var chargePercent = clampedTime / heavyMaxCharge;
        var energyCost = EnergyCost * heavyEnergyCostMultiplier * chargePercent;

        if (!CanShootHeavy(clampedTime, energyCost))
        {
            _heavyBullet.Delete();
            ShootBasicBullets();
            return;
        }

        ReleaseHeavyBullet(chargePercent);
        battleManager.UseEnergy(playerNum, energyCost);
        
        _cooldownTime = basicCooldownTime + clampedTime / heavyCooldownMultiplier;

        PlayHeavyBulletAudio();
    }

    public void ShootBasicBullets()
    {
        if (!CanShootBasic())
        {
            ShootWeakBullets();
            return;
        }

        TransformBullets(basicBulletPool, _damage, Speed);
        battleManager.UseEnergy(playerNum, EnergyCost);

        _cooldownTime = basicCooldownTime;

        PlayBulletAudio();
    }

    private void ShootWeakBullets()
    {
        // TODO: Add more feedback to player that they can't fire.
        // A sound perhaps?
        if (!CanShootWeak())
        {
            PlayCantShootParticles();
            return;
        }

        var damage = (int)Math.Round(this._damage * weakDamageMultiplier);
        var speed = Speed * weakSpeedMultiplier;
        TransformBullets(lowBulletPool, damage, speed);

        _cooldownTime = basicCooldownTime * weakCooldownMultiplier;

        PlayBulletAudio();
    }

    private void PlayCantShootParticles(bool ignoreMinCooldown = false)
    {
        if (!ignoreMinCooldown && _cooldownTime <= basicCooldownTime) return;
        if (particlesL.isStopped) particlesL.Play();
        if (particlesR.isStopped) particlesR.Play();
    }

    private void TransformBullets(ObjectPool bulletPool, float damage, float speed)
    {
        foreach (var shootingOrigin in shootingOrigins)
        {
            var bullet = bulletPool.GetPooledObject();

            bullet.transform.position = shootingOrigin.position;
            var direction = shootingOrigin.forward;

            bullet.TryGetComponent<Bullet>(out var bulletComponent);
            
            var firer = playerNum == 1 ? Firer.Player1 : Firer.Player2;
            bulletComponent.Init(damage, speed, direction, firer);
            bullet.SetActive(true);
        }
    }

    /// <summary>
    /// Spawn heavy bullet for charging.
    /// </summary>
    private void TransformHeavyBullet()
    {
        _heavyObject = heavyBulletPool.GetPooledObject();
        _heavyBullet = _heavyObject.GetComponent<Bullet>();
        
        _heavyObject.transform.position = CalculateHeavyPosition();

        var firer = playerNum == 1 ? Firer.Player1 : Firer.Player2;
        _heavyBullet.InitHeavy(CalculateHeavyForward(), firer);
        _heavyObject.SetActive(true);
    }

    private Vector3 CalculateHeavyPosition()
    {
        var positionL = shootingOrigins[0].position;
        var positionR = shootingOrigins[1].position;
        return positionL + (positionR - positionL) / 2;
    }

    private Vector3 CalculateHeavyForward()
    {
        var forwardL = shootingOrigins[0].forward;
        var forwardR = shootingOrigins[1].forward;
        return forwardL + (forwardR - forwardL) / 2;
    }

    private void ReleaseHeavyBullet(float chargePercent)
    {
        var damage = (int)Math.Floor(_damage * heavyDamageMultiplier * chargePercent);
        var speed = Speed * heavySpeedMultiplier / chargePercent;

        _heavyBullet.Fire(CalculateHeavyForward(), damage, speed);
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

    /// <summary>
    /// <p>Update values for charging heavy weapon.</p>
    /// <p>1. Update charged time</p>
    /// <p>2. Update bullet position and scale</p>
    /// <p>3. Update player movement speed</p>
    /// <p>4. Check if energy cost is over and stop early if needed</p>
    /// </summary>
    /// <param name="deltaTime">Time passed since last update.</param>
    private void UpdateHeavyCharge(float deltaTime)
    {
        if (!_isHeavyCharging) return;
        _heavyChargeTime += deltaTime;
        var clampedTime = Math.Clamp(_heavyChargeTime, 0, heavyMaxCharge);

        var scalar = clampedTime * heavySizeMultiplier;
        var scale = new Vector3(scalar, scalar, scalar);
        _heavyBullet.HoldHeavy(scale, CalculateHeavyPosition());

        var slowMultiplier = 1.5f - clampedTime / heavyMaxCharge;
        playerMovement.slowSpeed(slowMultiplier);

        var chargePercent = clampedTime / heavyMaxCharge;
        var energyCost = EnergyCost * heavyEnergyCostMultiplier * chargePercent;
        if (!battleManager.HasEnergy(playerNum, energyCost)) ShootHeavyBullet(clampedTime);
    }

    private void ResetHeavyCharge()
    {
        _isHeavyCharging = false;
        _heavyChargeTime = 0;
        playerMovement.resetMaxSpeed();
    }

    /// <summary>
    /// Sets player's current special move.
    /// </summary>
    /// <param name="specialMove">Special move to give player.</param>
    public void SetSpecialMove(PlayerSpecialMove specialMove)
    {
        _specialMove = specialMove;
    }

    /// <summary>
    /// Removes player's current special move.
    /// </summary>
    public void RemoveSpecialMove()
    {
        _specialMove = null;
    }

    void Update()
    {
        UpdateCooldown(Time.deltaTime);
        UpdateHeavyCharge(Time.deltaTime);
    }
}