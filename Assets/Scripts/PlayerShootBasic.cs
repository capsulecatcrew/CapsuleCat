using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShootBasic : MonoBehaviour
{
    private PlayerControls _playerControls;

    public ObjectPool bulletPool;
    private GameObject _bullet;

    public AudioClip shootingAudio;

    public AudioSource audioSource;

    public string[] tagsToHit;
    
    public Transform shootingOrigin;

    public Transform target;
    
    public float bulletSpeed = 10;

    public float bulletDespawnDist = 20;

    public int bulletDmg = 1;

    public float timeBetweenShots = 0.5f;
    private float _timeTillNexttShot;

    private Vector3 _bulletDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerControls = PlayerMovement.PlayerController;
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        _bulletDirection = target.position - shootingOrigin.position;
        _timeTillNexttShot = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _bulletDirection = target.position - shootingOrigin.position;
        if (_playerControls.Land.Attack.IsPressed() && _timeTillNexttShot < float.Epsilon)
        {
            _timeTillNexttShot = timeBetweenShots;
            _bullet = bulletPool.GetPooledObject();
            _bullet.transform.position = shootingOrigin.position;
            _bullet.GetComponent<Bullet>().Init(bulletDmg, _bulletDirection, bulletSpeed, bulletDespawnDist, tagsToHit);
            _bullet.SetActive(true);
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(shootingAudio);
        }

        if (_timeTillNexttShot > 0.0f)
        {
            _timeTillNexttShot -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(shootingOrigin.position, shootingOrigin.position + _bulletDirection, Color.cyan);
    }
}
