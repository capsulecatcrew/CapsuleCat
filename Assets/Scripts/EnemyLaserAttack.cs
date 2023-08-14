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

    public EnemyAttack[] bigAttacks;

    private GameObject _currLaser;
    private EnemyLaser _laserLogic;

    [Header("Laser Damage")]
    public int startingDamage = 2;
    private int _damage;
    public int dmgIncreaseLvlInterval = 5;
    public int dmgIncrease = 1;
    
    [Header("Shot Intervals")]
    public float startingMinTimeBetweenShots = 6;
    private float _minTimeBetweenShots;
    public float startingMaxTimeBetweenShots = 20;
    private float _maxTimeBetweenShots;
    public float minTimeLowerLimit = 1.5f;
    public float maxTimeLowerLimit = 3;
    public float timerDecreaseByLevel = 0.5f;
    
    private float _timeSinceLastLaser;
    private float _timeTillNextLaser;

    private int _bigAttackIndex;
    private bool _doingBigAttack = false;

    private void Awake()
    {
        _damage = startingDamage + dmgIncrease * PlayerStats.LevelsCompleted / dmgIncreaseLvlInterval;

        _minTimeBetweenShots = startingMinTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_minTimeBetweenShots < minTimeLowerLimit) _minTimeBetweenShots = minTimeLowerLimit;
        _maxTimeBetweenShots = startingMaxTimeBetweenShots - timerDecreaseByLevel * PlayerStats.LevelsCompleted;
        if (_maxTimeBetweenShots < maxTimeLowerLimit) _maxTimeBetweenShots = maxTimeLowerLimit;
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
        if (!_doingBigAttack && _timeSinceLastLaser > _timeTillNextLaser)
        {
            if (bigAttacks.Length > 0 && Random.Range(1, 11) > 1)
            {
                _doingBigAttack = true;
                _bigAttackIndex = Random.Range(0, bigAttacks.Length);
                bigAttacks[_bigAttackIndex].OnFinish += FinishedBigAttack;
                bigAttacks[_bigAttackIndex].StartAttack();
            }
            else
            {
            SingleAimedLaser();
            }

            ResetTimer();
        }
    }

    void SingleAimedLaser()
    {
        _currLaser = laserPool.GetPooledObject();
        _laserLogic = _currLaser.GetComponent<EnemyLaser>();
        _laserLogic.SetDamage(_damage);
        _laserLogic.SetTargetTracking(target.transform);
        _laserLogic.SetFiringTiming(firingDuration: 2);
        _currLaser.transform.position = transform.position;
        _currLaser.SetActive(true);
    }

    void FinishedBigAttack()
    {
        _doingBigAttack = false;
        bigAttacks[_bigAttackIndex].OnFinish -= FinishedBigAttack;
    }
    
    IEnumerator WaitBeforeExecution(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    void ResetTimer()
    {
        _timeSinceLastLaser = 0.0f;
        _timeTillNextLaser = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
    }
}
