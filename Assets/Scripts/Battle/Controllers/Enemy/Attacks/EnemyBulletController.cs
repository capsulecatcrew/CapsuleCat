using System;
using System.Collections.Generic;
using Battle;
using Enemy;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBulletController : MonoBehaviour
{
    [Header("Bullet Stats")]
    [SerializeField] private Transform origin;
    [SerializeField] private ObjectPool bulletPool;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int[] bulletAmounts;
    [SerializeField] private float[] bulletHeights;

    [Header("Cooldown Scaling")]
    [SerializeField] private float baseMinCooldown = 3;
    [SerializeField] private float minCooldownReduction = 0.3f;
    [SerializeField] private float baseMaxCooldown = 10;
    [SerializeField] private float maxCooldownReduction = 0.5f;
    [SerializeField] private float absoluteMinCooldown = 0.5f;
    private ClampedLinearStat _minCooldownTime;
    private ClampedLinearStat _maxCooldownTime;
    private RandomStat _cooldownTime;
    private float _cooldownTimer;
    
    [Header("Damage Scaling")]
    [SerializeField] private int baseDamage = 1;
    [SerializeField] private int damageIncrease = 1;
    [SerializeField] private int damageUpgradeInterval = 5;
    private IntervalLinearStat _damage;
    private float _damageValue;

    [Header("Audio")]
    [SerializeField] private EnemySoundController enemySoundController;
    
    public delegate void Attack();
    public event Attack OnAttack;

    public void Start()
    {
        if (origin == null)
        {
            origin = transform;
        }
        CreateStats();
        _damageValue = _damage.GetValue();
        _cooldownTimer = _cooldownTime.GenerateRandomValue();
    }

    private void CreateStats()
    {
        _minCooldownTime = new ClampedLinearStat("MinCd", int.MaxValue,
            baseMinCooldown,
            -minCooldownReduction, 0, 0,
            absoluteMinCooldown, float.MaxValue, false);
        _maxCooldownTime = new ClampedLinearStat("MaxCd", int.MaxValue,
            baseMaxCooldown,
            -maxCooldownReduction, 0, 0,
            absoluteMinCooldown, float.MaxValue, false);
        _damage = new IntervalLinearStat("Damage", int.MaxValue,
            baseDamage,
            damageIncrease, 0, 0,
            damageUpgradeInterval, false);
        _minCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
        _maxCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
        _damage.SetLevel(PlayerStats.GetCurrentStage());
        _cooldownTime = new RandomStat(_minCooldownTime, _maxCooldownTime);
    }
    
    public void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        if (_cooldownTimer > float.Epsilon) return;
        SpawnBullets();
        _cooldownTimer = _cooldownTime.GenerateRandomValue();
    }
    
    private void SpawnBullets()
    {
        var directions = GetBulletDirections();
        var position = GetBulletPosition();
        foreach (var direction in directions)
        {
            var bullet = bulletPool.GetPooledObject();
            bullet.transform.position = position;
            bullet.GetComponent<Bullet>()?.Init(_damageValue, bulletSpeed, direction, Firer.Enemy);
            bullet.SetActive(true);
        }
        OnAttack?.Invoke();
        enemySoundController.PlayBulletSound();
    }

    private Vector3 GetBulletPosition()
    {
        var height = bulletHeights[Random.Range(0, bulletHeights.Length)];
        var position = origin.position;
        return new Vector3(position.x, height, position.z);
    }

    private IEnumerable<Vector3> GetBulletDirections()
    {
        var amount = bulletAmounts[Random.Range(0, bulletAmounts.Length)];
        var result = new Vector3[amount];
        var theta = 2 * (float) Math.PI / amount;
        var startingAngle = Random.Range(0.0f, theta);
        for (var i = 0; i < amount; i++)
        {
            result[i] = new Vector3(Mathf.Sin(startingAngle + theta * i), 0, Mathf.Cos(startingAngle + theta * i));
        }
        return result;
    }
}
