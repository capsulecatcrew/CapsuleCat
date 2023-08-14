using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class PlayerShootBasic : MonoBehaviour
{
    private PlayerControls _playerControls;

    public PlayerEnergy playerEnergy;
    
    public ObjectPool bulletPool;
    public ObjectPool lowBulletPool;
    private GameObject _bullet;

    public AudioClip shootingAudio;

    public AudioSource audioSource;

    public string[] tagsToHit;
    
    public Transform shootingOrigin;

    public Transform target;
    
    public float bulletSpeed = 10;

    public float bulletDespawnDist = 20;

    public int bulletDmg = 2;
    public int noEnergyPenalty = 3;
    
    public float timeBetweenShots = 0.5f;
    private float _timeTillNextShot;

    private Vector3 _bulletDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerControls = PlayerMovement.PlayerController;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        _bulletDirection = target.position - shootingOrigin.position;
        _timeTillNextShot = 0;
        timeBetweenShots = PlayerStats.FiringRate.GetCurrentValue();
    }

    // Update is called once per frame
    void Update()
    {
        _bulletDirection = target.position - shootingOrigin.position;
        if (_playerControls.Player1.Attack.IsPressed() && _timeTillNextShot < float.Epsilon)
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
            
            // initialise bullet
            _bullet = noEnergy ? lowBulletPool.GetPooledObject() : bulletPool.GetPooledObject();
            _bullet.transform.position = shootingOrigin.position;
            _bullet.GetComponent<Bullet>().Init(dmg, _bulletDirection, spd, bulletDespawnDist, tagsToHit);
            _bullet.SetActive(true);
            
            // reduce player energy
            if (!noEnergy) playerEnergy.AddAmount(-1);
            
            // Play shooting audio
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(shootingAudio);
        }

        if (_timeTillNextShot > 0.0f)
        {
            _timeTillNextShot -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(shootingOrigin.position, shootingOrigin.position + _bulletDirection, Color.cyan);
    }
}
