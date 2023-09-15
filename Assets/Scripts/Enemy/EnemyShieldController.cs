using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShieldController : MonoBehaviour
{
    public enum RotateDirection
    {
        Clockwise,
        Anticlockwise
    }
    
    [SerializeField] private GameObject[] shields;
    private const int MaxShieldCount = 12;
    private int _shieldCount;

    private static readonly LinearStat StatMaxHealth = new ("Health", int.MaxValue, 5, 2, 0, 0);

    private RotateDirection _rotateDirection;
    private static readonly ExponentialStat StatRotateSpeed = new("Rotate", int.MaxValue, 20, 1.2f, 0, 100, 0, 0);
    
    private float _pauseTime;
    private float _pauseCooldown;

    private static readonly ExponentialStat StatMinPauseTime =
        new("Min Pause", int.MaxValue, 2, 0.8f, float.MaxValue, 0.5f, 0, 0);
    private static readonly ExponentialStat StatMaxPauseTime =
        new("Max Pause", int.MaxValue, 60, 0.8f, float.MaxValue, 0.6f, 0, 0);
    
    private static readonly ExponentialStat StatMinCooldownTime =
        new("Min Cooldown", int.MaxValue, 3, 0.8f, float.MaxValue, 0.5f, 0, 0);
    private static readonly ExponentialStat StatMaxCooldownTime =
        new("Max Cooldown", int.MaxValue, 10, 0.8f, float.MaxValue, 0.5f, 0, 0);

    public void Awake()
    {
        var currentStage = PlayerStats.GetCurrentStage();
        _shieldCount = Random.Range(currentStage / 4, currentStage);
        if (_shieldCount > MaxShieldCount) _shieldCount = MaxShieldCount;

        StatMaxHealth.SetLevel(currentStage);
        StatRotateSpeed.SetLevel(currentStage);
        StatMinPauseTime.SetLevel(currentStage);
        StatMaxPauseTime.SetLevel(currentStage);
        StatMinCooldownTime.SetLevel(currentStage);
        StatMaxCooldownTime.SetLevel(currentStage);

        PickRandomDirection();
        UpdatePauseTime();
    }

    public void Start()
    {
        GenerateShields();
    }

    public void Update()
    {
        UpdatePauseTime();
    }

    public void GenerateShields()
    {
        DisableShields();
        
        var possibleShieldGroupNums = new List<int> { 1 };
        if (_shieldCount % 2 == 0) possibleShieldGroupNums.Add(2);
        if (_shieldCount % 3 == 0) possibleShieldGroupNums.Add(3);
        if (_shieldCount % 4 == 0) possibleShieldGroupNums.Add(4);
        
        var shieldGroups = possibleShieldGroupNums[Random.Range(0, possibleShieldGroupNums.Count)];
        var shieldsPerGroup = _shieldCount / shieldGroups;

        for (int i = 0; i < MaxShieldCount; i += shieldsPerGroup)
        {
            for (int j = 0; j < shieldsPerGroup; j++)
            {
                GameObject shield = shields[i + j];
                shield.SetActive(true);
            }
        }
    }
    
    private void PickRandomDirection()
    {
        _rotateDirection = Random.Range(0, 2) switch
        {
            1 => RotateDirection.Clockwise,
            2 => RotateDirection.Anticlockwise,
            _ => RotateDirection.Clockwise
        };
    }

    public static LinearStat GetMaxHealthStat()
    {
        return StatMaxHealth;
    }

    private void DisableShields()
    {
        foreach (var shield in shields)
        {
            shield.SetActive(false);
        }
    }

    private void RotateShields()
    {
        var deltaTime = Time.deltaTime;
        foreach (var shield in shields)
        {
            RotateShield(shield, deltaTime);
        }
        _pauseCooldown -= deltaTime;
        
        if (_pauseCooldown < float.Epsilon)
        {
            PauseRotateShields();
        }
    }

    private void RotateShield(GameObject shield, float deltaTime)
    {
        switch (_rotateDirection)
        {
            case RotateDirection.Clockwise:
                shield.transform.Rotate(0, StatRotateSpeed.GetValue() * deltaTime, 0);
                break;
            case RotateDirection.Anticlockwise:
                shield.transform.Rotate(0, -StatRotateSpeed.GetValue() * deltaTime, 0);
                break;
            default:
                return;
        }
    }

    private void PauseRotateShields()
    {
        _pauseTime = Random.Range(StatMinPauseTime.GetValue(), StatMaxPauseTime.GetValue());
        if (Random.Range(0, 3) > 0) PickRandomDirection();
    }

    private void UpdatePauseTime()
    {
        _pauseTime -= Time.deltaTime;
        if (_pauseTime > 0) return;
        RotateShields();
        _pauseCooldown = Random.Range(StatMinCooldownTime.GetValue(), StatMaxCooldownTime.GetValue());
    }
}