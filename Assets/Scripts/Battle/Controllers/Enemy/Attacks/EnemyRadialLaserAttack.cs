using System;
using System.Collections;
using Enemy;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(EnemyLaserController))]
public class EnemyRadialLaserAttack : EnemyAttack
{
    [Header("Laser Damage")]
    public int startingDamage = 2;
    private int _damage;
    public int dmgIncreaseLvlInterval = 5;
    public int dmgIncrease = 1;

    [Header("Attack Parameters")]
    public int startingNumOfLasers = 3;
    public int maxNumOfLasers = 7;
    public int lasersIncreaseLvlInterval = 5;
    public int lowerLaserHeight, upperLaserHeight;
    private int _numOfLasers;

    public float rotationSpeed;

    // private references
    private ObjectPool _laserPool;
    private ObjectPool _specialLaserPool;
    private GameObject _currLaser;
    private EnemyLaser _laserLogic;
    private ArrayList _lasersInUse;

    // other private variables
    private bool _isSpinning;
    private int _spinDir = 1;

    public void Start()
    {
        _lasersInUse = new ArrayList();
        _laserPool = GetComponent<EnemyLaserController>().laserPool;
        _specialLaserPool = GetComponent<EnemyLaserController>().specialLaserPool;
        _damage = startingDamage + dmgIncrease * PlayerStats.GetCurrentStage() / dmgIncreaseLvlInterval;
        _numOfLasers = Math.Clamp(startingNumOfLasers + PlayerStats.GetCurrentStage() / lasersIncreaseLvlInterval, 0,
            maxNumOfLasers);
    }

    public override void StartAttack()
    {
        _spinDir = Random.Range(0, 2) == 1 ? 1 : -1;
        RadialLasers(_damage, _numOfLasers);
        StartCoroutine(StartSpinning());
        StartCoroutine(FinishAttack());
    }

    private void RadialLasers(int damage, int numOfLasers, float arcAngle = 360)
    {
        if (arcAngle > 360 || arcAngle < 0) arcAngle = 360;

        float theta = 0;
        if (arcAngle == 360 || numOfLasers == 1)
        {
            theta = Mathf.Deg2Rad * arcAngle / numOfLasers;
        }
        else
        {
            theta = Mathf.Deg2Rad * arcAngle / (numOfLasers - 1);
        }

        float startingAngle = Random.Range(0.0f, theta);
        float distToPlayer = 30;
        // another time.
        for (int i = 0; i < numOfLasers; i++)
        {
            float targetHeight = Random.Range(1, 4) % 2 == 0 && i != 0 ? upperLaserHeight : lowerLaserHeight;
            Vector3 dir = new Vector3(distToPlayer * Mathf.Sin(startingAngle + theta * i), targetHeight,
                distToPlayer * Mathf.Cos(startingAngle + theta * i));
            if (Random.Range(1, 11) > 9) // 10% chance of special laser
            {
                _currLaser = _specialLaserPool.GetPooledObject();
            }
            else
            {
                _currLaser = _laserPool.GetPooledObject();
            }

            _laserLogic = _currLaser.GetComponent<EnemyLaser>();
            _laserLogic.Init(damage, dir, 5.02f);
            _currLaser.SetActive(true);
            _lasersInUse.Add(_currLaser);
        }
    }

    private IEnumerator StartSpinning()
    {
        yield return new WaitForSeconds(0.5f);
        _isSpinning = true;
        yield return null;
    }

    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(8.5f);
        _isSpinning = false;
        _lasersInUse.Clear();
        DeclareAttackDone();
        yield return null;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!_isSpinning) return;
        foreach (GameObject laser in _lasersInUse)
        {
            laser.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * _spinDir * Time.deltaTime, Space.World);
        }
    }
}