using System.Collections.Generic;
using System.Linq;
using Player.Stats.Persistent;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyShieldController : MonoBehaviour
{
    private enum RotateDirection
    {
        Clockwise,
        Anticlockwise
    }

    private RotateDirection _rotateDirection;
    private static readonly UpgradeableExponentialStat RotateSpeed = new("Rotate", int.MaxValue, 20, 1.2f, 0, 0);

    private const int MaxShieldCount = 12;
    private int _activeShieldCount;
    [SerializeField] private GameObject[] shields;

    private readonly UpgradeableLinearStat _maxHealth = new("Health", int.MaxValue, 5, 2, 0, 0);
    private readonly Dictionary<GameObject, BattleStat> _shieldHealths = new();

    [SerializeField] private Color maxHpColor;
    [SerializeField] private Color minHpColor;

    private float _pause = 5;

    private static readonly ClampedExponentialStat MinPauseTime =
        new("Min Pause", int.MaxValue, 2, 0.8f, 0, 0, 0.5f, float.MaxValue);

    private static readonly ClampedExponentialStat MaxPauseTime =
        new("Max Pause", int.MaxValue, 60, 0.8f, 0, 0, 0.6f, float.MaxValue);

    private RandomStat _pauseRandom;

    private float _pauseCooldown;

    private static readonly ClampedExponentialStat MinCooldownTime =
        new("Min Cooldown", int.MaxValue, 3, 0.8f, 0, 0, 0.5f, float.MaxValue);

    private static readonly ClampedExponentialStat MaxCooldownTime =
        new("Max Cooldown", int.MaxValue, 10, 0.8f, 0, 0, 0.5f, float.MaxValue);

    private RandomStat _pauseCooldownRandom;

    public void Awake()
    {
        var currentStage = PlayerStats.GetCurrentStage();
        _activeShieldCount = Random.Range(currentStage / 4, currentStage);
        if (_activeShieldCount > MaxShieldCount) _activeShieldCount = MaxShieldCount;

        _maxHealth.SetLevel(currentStage);
        RotateSpeed.SetLevel(currentStage);
        MinPauseTime.SetLevel(currentStage);
        MaxPauseTime.SetLevel(currentStage);
        MinCooldownTime.SetLevel(currentStage);
        MaxCooldownTime.SetLevel(currentStage);

        _pauseRandom = new RandomStat(MinPauseTime, MaxPauseTime);
        _pauseCooldownRandom = new RandomStat(MinCooldownTime, MaxCooldownTime);
    }

    public void Start()
    {
        GenerateShields();
    }

    public void Update()
    {
        _pause -= Time.deltaTime;
        if (_pause > 0) return;
        RotateShields();
        _pauseCooldown = _pauseCooldownRandom.GenerateRandomValue();
    }

    private void GenerateShields()
    {
        DisableShields();
        if (_activeShieldCount == 0) return;

        var possibleShieldGroupNums = new List<int> { 1 };
        if (_activeShieldCount % 2 == 0) possibleShieldGroupNums.Add(2);
        if (_activeShieldCount % 3 == 0) possibleShieldGroupNums.Add(3);
        if (_activeShieldCount % 4 == 0) possibleShieldGroupNums.Add(4);

        var shieldGroups = possibleShieldGroupNums[Random.Range(0, possibleShieldGroupNums.Count)];
        if (shieldGroups == 0) return;
        var shieldsPerGroup = _activeShieldCount / shieldGroups;

        for (var i = 0; i < MaxShieldCount; i += MaxShieldCount / shieldGroups)
        {
            for (var j = 0; j < shieldsPerGroup; j++)
            {
                var shield = shields[i + j];
                BattleStat shieldHealth = _maxHealth.CreateBattleStat();
                _shieldHealths.Add(shield, shieldHealth);
                shieldHealth.SetGameObjectToKill(shield);
                shield.SetActive(true);
            }
        }
    }

    public bool IsEnemyShield(GameObject hitObject)
    {
        return shields.Contains(hitObject);
    }

    public void HitEnemyShield(GameObject hitObject, float damage, bool ignoreIFrames)
    {
        var shieldHealth = _shieldHealths[hitObject];
        shieldHealth.MinusValue(damage, ignoreIFrames);
        hitObject.GetComponent<Renderer>().material.color = GetDamageColor(shieldHealth.GetStatPercentage());
    }

    private Color GetDamageColor(float percentage)
    {
        return Color.Lerp(minHpColor, maxHpColor, percentage);
    }

    private void PickRandomDirection()
    {
        _rotateDirection = Random.Range(0, 2) switch
        {
            0 => RotateDirection.Clockwise,
            1 => RotateDirection.Anticlockwise,
            _ => RotateDirection.Clockwise
        };
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
        foreach (var shield in _shieldHealths.Keys)
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
                shield.transform.Rotate(0, RotateSpeed.GetValue() * deltaTime, 0);
                break;
            case RotateDirection.Anticlockwise:
                shield.transform.Rotate(0, -RotateSpeed.GetValue() * deltaTime, 0);
                break;
            default:
                return;
        }
    }

    private void PauseRotateShields()
    {
        _pause = _pauseRandom.GenerateRandomValue();
        if (Random.Range(0, 3) > 0) PickRandomDirection();
    }
}