using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShieldController : MonoBehaviour
{
    private enum RotateDirection
    {
        Clockwise,
        Anticlockwise
    }
    
    [SerializeField] private GameObject[] shields;
    private const int MaxShieldCount = 12;
    private int _shieldCount;

    private readonly LinearStat _statMaxHealth = new ("Health", int.MaxValue, 5, 2, 0, 0);

    private RotateDirection _rotateDirection;
    private readonly ExponentialStat _statRotateSpeed = new("Rotate", int.MaxValue, 20, 1.2f, 0, 100, 0, 0);
    
    private float _pauseTime;
    private float _pauseCooldown;

    private readonly ExponentialStat _statMinPauseTime =
        new("Min Pause", int.MaxValue, 2, 0.8f, float.MaxValue, 0.5f, 0, 0);
    private readonly ExponentialStat _statMaxPauseTime =
        new("Max Pause", int.MaxValue, 60, 0.8f, float.MaxValue, 0.6f, 0, 0);
    
    private readonly ExponentialStat _statMinCooldownTime =
        new("Min Cooldown", int.MaxValue, 3, 0.8f, float.MaxValue, 0.5f, 0, 0);
    private readonly ExponentialStat _statMaxCooldownTime =
        new("Max Cooldown", int.MaxValue, 10, 0.8f, float.MaxValue, 0.5f, 0, 0);

    public void Awake()
    {
        var currentStage = PlayerStats.GetCurrentStage();
        _shieldCount = Random.Range(currentStage / 4, currentStage);
        if (_shieldCount > MaxShieldCount) _shieldCount = MaxShieldCount;

        _statMaxHealth.SetLevel(currentStage);
        _statRotateSpeed.SetLevel(currentStage);
        _statMinPauseTime.SetLevel(currentStage);
        _statMaxPauseTime.SetLevel(currentStage);
        _statMinCooldownTime.SetLevel(currentStage);
        _statMaxCooldownTime.SetLevel(currentStage);

        PauseRotateShields();
    }

    public void Start()
    {
        GenerateShields();
    }

    public void Update()
    {
        _pauseTime -= Time.deltaTime;
        if (_pauseTime > 0) return;
        RotateShields();
        _pauseCooldown = Random.Range(_statMinCooldownTime.GetValue(), _statMaxCooldownTime.GetValue());
    }

    private void GenerateShields()
    {
        DisableShields();
        
        var possibleShieldGroupNums = new List<int> { 1 };
        if (_shieldCount % 2 == 0) possibleShieldGroupNums.Add(2);
        if (_shieldCount % 3 == 0) possibleShieldGroupNums.Add(3);
        if (_shieldCount % 4 == 0) possibleShieldGroupNums.Add(4);
        
        var shieldGroups = possibleShieldGroupNums[Random.Range(0, possibleShieldGroupNums.Count)];
        var shieldsPerGroup = _shieldCount / shieldGroups;

        for (var i = 0; i < MaxShieldCount; i += shieldsPerGroup)
        {
            for (var j = 0; j < shieldsPerGroup; j++)
            {
                var shield = shields[i + j];
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

    public LinearStat GetMaxHealthStat()
    {
        return _statMaxHealth;
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
                shield.transform.Rotate(0, _statRotateSpeed.GetValue() * deltaTime, 0);
                break;
            case RotateDirection.Anticlockwise:
                shield.transform.Rotate(0, -_statRotateSpeed.GetValue() * deltaTime, 0);
                break;
            default:
                return;
        }
    }

    private void PauseRotateShields()
    {
        _pauseTime = Random.Range(_statMinPauseTime.GetValue(), _statMaxPauseTime.GetValue());
        if (Random.Range(0, 3) > 0) PickRandomDirection();
    }
}