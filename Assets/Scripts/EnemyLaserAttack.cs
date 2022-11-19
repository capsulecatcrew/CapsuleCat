using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyLaserAttack : MonoBehaviour
{
    public GameObject target;
    public ObjectPool laserPool;
    private GameObject _currLaser;
    private EnemyLaser _laserLogic;

    public int startingDamage = 2;
    private int _damage;
    public int dmgIncreaseLvlInterval = 5;
    public int dmgIncrease = 1;
    public float startingMinTimeBetweenShots = 6;
    private float _minTimeBetweenShots;
    public float startingMaxTimeBetweenShots = 20;
    private float _maxTimeBetweenShots;
    public float absoluteMinTimeBetweenShots = 1.5f;
    public float timerDecreaseByLevel = 0.5f;
    
    private float _timeSinceLastLaser;
    private float _timeTillNextLaser;

    private void Awake()
    {
        _damage = startingDamage + dmgIncrease * PlayerStats.LevelsCompleted / dmgIncreaseLvlInterval;

        _minTimeBetweenShots = startingMinTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_minTimeBetweenShots < absoluteMinTimeBetweenShots) _minTimeBetweenShots = absoluteMinTimeBetweenShots;
        _maxTimeBetweenShots = startingMaxTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_maxTimeBetweenShots < absoluteMinTimeBetweenShots) _maxTimeBetweenShots = absoluteMinTimeBetweenShots;
    }

    // Start is called before the first frame update
    void Start()
    {
        _timeSinceLastLaser = 0.0f;
        _timeTillNextLaser = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        _timeSinceLastLaser += Time.deltaTime;
        if (_timeSinceLastLaser > _timeTillNextLaser)
        {
            SingleAimedLaser();
            ResetTimer();
        }
    }

    void SingleAimedLaser()
    {
        _currLaser = laserPool.GetPooledObject();
        _laserLogic = _currLaser.GetComponent<EnemyLaser>();
        _laserLogic.SetDamage(_damage);
        _laserLogic.SetTargetTracking(target.transform);
        _currLaser.transform.position = transform.position;
        _currLaser.SetActive(true);
    }

    void RadialLasers(int numOfLasers, Vector3 arcCenter, float arcAngle = 360)
    {
        // another time.
    }
    
    void ResetTimer()
    {
        _timeSinceLastLaser = 0.0f;
        _timeTillNextLaser = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
    }
}
