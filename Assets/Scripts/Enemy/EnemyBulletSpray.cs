using System;
using Battle;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBulletSpray : MonoBehaviour
{
    public Transform origin;

    public ObjectPool bulletPool;
    private GameObject _bullet;

    [Header("Level Scaling")]
    public float startingMinTimeBetweenShots = 3;
    private float _minTimeBetweenShots;
    public float startingMaxTimeBetweenShots = 10;
    private float _maxTimeBetweenShots;
    public float absoluteMinTimeBetweenShots = 0.5f;
    public float timerDecreaseByLevel = 0.5f;
    
    public int startingDamage = 1;
    private int _damage;
    public int dmgIncreaseLvlInterval = 5;
    public int dmgIncrease = 1;

    public int[] bulletAmounts;
    private int _amountsCount;
    public float[] bulletHeights;
    private int _heightsCount;
    
    public float bulletSpeed;

    private float _timeSinceLastAttack;
    private float _timeTillNextAttack;

    public void Start()
    {
        if (origin == null)
        {
            origin = transform;
        }

        _damage = startingDamage + dmgIncrease * PlayerStats.GetCurrentStage() / dmgIncreaseLvlInterval;
        _timeSinceLastAttack = 0;
        _minTimeBetweenShots = startingMinTimeBetweenShots - timerDecreaseByLevel * PlayerStats.GetCurrentStage();
        if (_minTimeBetweenShots < absoluteMinTimeBetweenShots) _minTimeBetweenShots = absoluteMinTimeBetweenShots;
        _maxTimeBetweenShots = startingMaxTimeBetweenShots - timerDecreaseByLevel * PlayerStats.GetCurrentStage();
        if (_maxTimeBetweenShots < absoluteMinTimeBetweenShots) _maxTimeBetweenShots = absoluteMinTimeBetweenShots;

        _timeTillNextAttack = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
        _heightsCount = bulletHeights.Length;
        _amountsCount = bulletAmounts.Length;
    }

    // Update is called once per frame
    public void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
        if (_timeSinceLastAttack <= _timeTillNextAttack) return;
        // Attack
        var height = bulletHeights[Random.Range(0, _heightsCount)];
        var amount = bulletAmounts[Random.Range(0, _amountsCount)];
        var theta = 2 * (float) Math.PI / amount;
        // random starting angle so the "empty spots"
        // without bullets aren't the same when the bullet count is the same
        var startingAngle = Random.Range(0.0f, theta);
        var position = origin.position;
        position = new Vector3(position.x, height, position.z);
        for (var i = 0; i < amount; i++)
        {
            var dir = new Vector3(Mathf.Sin(startingAngle + theta * i), 0, Mathf.Cos(startingAngle + theta * i));
            _bullet = bulletPool.GetPooledObject();
            _bullet.transform.position = position;
            _bullet.GetComponent<Bullet>().Init(_damage, bulletSpeed, dir, Firer.Enemy);
            _bullet.SetActive(true);
        }
        GlobalAudio.Singleton.PlaySound("ENEMY_BULLET_WAVE_PULSE");
        _timeTillNextAttack = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
        _timeSinceLastAttack = 0.0f;
    }
}
