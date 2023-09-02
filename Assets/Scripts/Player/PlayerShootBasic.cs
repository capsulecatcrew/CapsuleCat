using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PlayerShootBasic : MonoBehaviour
{
    public GameObject[] weapons;
    private PlayerControls _playerControls;

    public PlayerEnergy playerEnergy;
    
    public ObjectPool bulletPool;
    public ObjectPool lowBulletPool;
    private GameObject _bullet;

    public AudioClip shootingAudio;

    public AudioSource audioSource;

    public string[] tagsToHit;
    
    public Transform[] shootingOrigins;

    public Transform target;
    
    public float bulletSpeed = 10;

    public float aimSpeed = 20;
    public float aimXLimit = 35;
    public float aimYLimit = 45;

    public float bulletDespawnDist = 20;

    public int bulletDmg = 2;
    public int noEnergyPenalty = 3;
    
    // Time in seconds between each basic attack
    public float timeBetweenShots = 0.5f;
    private float _timeTillNextShot;

    private Vector3 _bulletDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerControls = PlayerMovement.PlayerController;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        // _bulletDirection = shootingOrigin.forward;
        _timeTillNextShot = 0;
        timeBetweenShots = PlayerStats.FiringRate.GetCurrentValue();
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        foreach (Transform shootingOrigin in shootingOrigins)
        {
            Debug.DrawLine(shootingOrigin.position, shootingOrigin.position + shootingOrigin.forward, Color.cyan);
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
                gun.transform.localRotation= Quaternion.Euler(gun.transform.rotation.eulerAngles.x, aimYLimit, 0);
            }
            else if (yRot > 180 && yRot < 360 - aimYLimit)
            {
                gun.transform.localRotation = Quaternion.Euler(gun.transform.rotation.eulerAngles.x, 360 - aimYLimit, 0);
            }

        }
    }

    /// <summary>
    /// Shoots bullets.
    /// </summary>
    public void ShootBullet()
    {
        if (_timeTillNextShot < float.Epsilon)
        {
            bool noEnergy = playerEnergy.IsEmpty();
            // reset shooting timer
            _timeTillNextShot = timeBetweenShots;
            
            // set up bullet damage
            int dmg = bulletDmg;
            float spd = bulletSpeed;
            // reduce fire rate and bullet damage if player has no energy
            if (noEnergy)
            {
                _timeTillNextShot *= noEnergyPenalty;
                dmg /= noEnergyPenalty;
                spd /= noEnergyPenalty;
                // make sure damage is at least 1.
                dmg = Math.Max(dmg, 1);
            }
            
            // initialise bullets
            foreach (Transform shootingOrigin in shootingOrigins)
            {
                _bullet = noEnergy ? lowBulletPool.GetPooledObject() : bulletPool.GetPooledObject();
                _bullet.transform.position = shootingOrigin.position;
                _bulletDirection = shootingOrigin.forward;
                _bullet.GetComponent<Bullet>().Init(dmg, _bulletDirection, spd, bulletDespawnDist, tagsToHit);
                _bullet.SetActive(true);
            }
            
            // reduce player energy
            if (!noEnergy) playerEnergy.AddAmount(-1);
            
            // Play shooting audio
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(shootingAudio);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (_timeTillNextShot > 0.0f)
        {
            _timeTillNextShot -= Time.deltaTime;
        }
    }
}
