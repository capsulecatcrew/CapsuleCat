using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShoot : MonoBehaviour
{
    public GameObject[] weapons;
    private PlayerControls _playerControls;

    public PlayerEnergy playerEnergy;

    public ObjectPool bulletPool;
    public ObjectPool lowBulletPool;

    public AudioClip shootingAudio;

    public AudioSource audioSource;

    public string[] tagsToHit;

    public Transform[] shootingOrigins;

    public Transform target;

    public float aimSpeed = 20;
    public float aimXLimit = 35;
    public float aimYLimit = 45;

    // General bullet info - start at 0.5 so that player can't shoot until level is loaded
    public float bulletCooldown = 0.5f;

    // Basic bullet fields
    public int bulletDmg = 2;
    public float bulletSpeed = 10;
    public float bulletDespawnDist = 20;
    private float _bulletCooldownTime;

    // Weak bullet fields
    public float weakBulletDamageMultiplier = 0.7f;
    public float weakBulletSpeedMultiplier = 0.5f;
    public float weakBulletCooldownMultiplier = 3.0f;

    // Heavy bullet fields
    private bool _isHeavyBullet;
    private float _heavyBulletCharge;
    private float _heavyBulletMaxCharge = 3.0f;
    private float _heavyBulletIdle;
    private float _heavyBulletMaxIdle = 1.5f;
    private float _heavyBulletDamageMultiplier = 1.5f;
    private float _heavyBulletSpeedMultiplier = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _playerControls = PlayerMovement.PlayerController;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        // _bulletDirection = shootingOrigin.forward;
        _bulletCooldownTime = 0;
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
    /// Shoots bullets.
    /// </summary>
    public void ShootBullet()
    {
        // Prevent shooting if player is charging heavy attack or if insufficient time has passed.
        if (_bulletCooldownTime >= float.Epsilon) return;
        
        // Change to this once heavy shots are introduced.
        // if (_bulletCooldownTime >= float.Epsilon || !_isHeavyAttack) return;

        // All clear, shoot bullets.
        if (playerEnergy.IsEmpty())
        {
            ShootWeakBullets();
        }
        else
        {
            ShootBasicBullets();
        }

        // All done - play shooting audio
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(shootingAudio);
    }

    public void ShootHeavyBullets()
    {
        // charge heavy attack if attack button held for 2 turns in a row
        if (_isHeavyBullet)
        {
            _heavyBulletCharge += Time.deltaTime;
        }
        else
        {
            if (_heavyBulletCharge > 0)
            {
                _heavyBulletCharge = 0;
            }
        }

        // toggle heavy attack status if charging now
        if (!_isHeavyBullet)
        {
            _isHeavyBullet = true;
        }

        if (_heavyBulletCharge > _heavyBulletMaxCharge)
        {
        }
    }

    private void ShootBasicBullets()
    {
        foreach (Transform shootingOrigin in shootingOrigins)
        {
            GameObject bullet = bulletPool.GetPooledObject();

            bullet.transform.position = shootingOrigin.position;
            Vector3 direction = shootingOrigin.forward;

            bullet.GetComponent<Bullet>().Init(bulletDmg, direction, bulletSpeed, bulletDespawnDist, tagsToHit);
            bullet.SetActive(true);
        }

        playerEnergy.AddAmount(-1);
        _bulletCooldownTime = bulletCooldown;
    }

    private void ShootWeakBullets()
    {
        int damage = (int)Math.Round(bulletDmg * weakBulletDamageMultiplier);
        float speed = bulletSpeed * weakBulletSpeedMultiplier;
        float cooldown = bulletCooldown * weakBulletCooldownMultiplier;

        foreach (Transform shootingOrigin in shootingOrigins)
        {
            GameObject bullet = bulletPool.GetPooledObject();

            bullet.transform.position = shootingOrigin.position;
            Vector3 direction = shootingOrigin.forward;

            bullet.GetComponent<Bullet>().Init(damage, direction, speed, bulletDespawnDist, tagsToHit);
            bullet.SetActive(true);
        }

        _bulletCooldownTime = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (_bulletCooldownTime <= 0.0f) return;
        _bulletCooldownTime -= Time.deltaTime;
        // _heavyBulletIdle += Time.deltaTime;
        // if (_heavyBulletIdle > _heavyBulletMaxIdle)
        // {
        //     _isHeavyBullet = false;
        // }
    }
}