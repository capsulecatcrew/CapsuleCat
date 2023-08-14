using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyLaserAttack))]
public class EnemyRadialLaserAttack : EnemyAttack
{
    public int damage = 1;
    public int numOfLasers = 3;
    public float rotationSpeed = 0;

    // private references
    private ObjectPool _laserPool;
    private GameObject _currLaser;
    private EnemyLaser _laserLogic;
    private ArrayList _lasersInUse;

    // private variables
    private bool _isSpinning = false;
    private int _spinDir = 1;

    void Start()
    {
        _lasersInUse = new ArrayList();
        _laserPool = GetComponent<EnemyLaserAttack>().laserPool;
    }

    public override void StartAttack()
    {
        _spinDir = Random.Range(0, 2) == 1 ? 1 : -1;
        RadialLasers(damage, numOfLasers);
        StartCoroutine(StartSpinning());
        StartCoroutine(FinishAttack());
    }

    void RadialLasers(int damage, int numOfLasers, float arcAngle = 360)
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
        float targetHeight = 1;
        // another time.
        for (int i = 0; i < numOfLasers; i++)
        {
            Vector3 dir = new Vector3(distToPlayer * Mathf.Sin(startingAngle + theta * i), targetHeight, distToPlayer * Mathf.Cos(startingAngle + theta * i));
            _currLaser = _laserPool.GetPooledObject();
            _laserLogic = _currLaser.GetComponent<EnemyLaser>();
            _laserLogic.SetDamage(damage);
            _laserLogic.DisableTargetTracking();
            _laserLogic.SetTargetPosition(dir);
            _laserLogic.SetFiringTiming(firingDuration: 5);
            _currLaser.SetActive(true);
            _lasersInUse.Add(_currLaser);
        }
    }

    IEnumerator StartSpinning()
    {
        yield return new WaitForSeconds(3);
        _isSpinning = true;
        yield return null;
    }

    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(8.5f);
        // _isSpinning = false;
        _lasersInUse.Clear();
        DeclareAttackDone();
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSpinning)
        {
            foreach (GameObject laser in _lasersInUse)
            {
                laser.transform.Rotate(new Vector3(0, 1, 0) * rotationSpeed * _spinDir * Time.deltaTime, Space.World);
                // laser.transform.rotation *= Quaternion.AngleAxis(rotationSpeed * _spinDir * Time.deltaTime, Vector3.up);
            }
        }
    }
}
