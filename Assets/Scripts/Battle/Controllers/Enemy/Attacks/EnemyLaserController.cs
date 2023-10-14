using Enemy;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyLaserController : MonoBehaviour
{
    public GameObject target;
    public ObjectPool laserPool;
    public ObjectPool specialLaserPool;

    public EnemyAttack[] bigAttacks;

    private GameObject _currLaser;
    private EnemyLaser _laserLogic;

    [Header("Laser Damage")]
    [SerializeField] private int startingDamage = 4;
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
    private bool _doingBigAttack;
    
    [Header("Audio")]
    [SerializeField] private EnemySoundController enemySoundController;

    public delegate void Attack();
    public event Attack OnAttack;
    
    /// <summary>
    /// Chance of firing a special laser out of 100.
    /// </summary>
    [SerializeField] private int specialLaserChance;

    public void Awake()
    {
        _damage = startingDamage + dmgIncrease * PlayerStats.GetCurrentStage() / dmgIncreaseLvlInterval;

        _minTimeBetweenShots = startingMinTimeBetweenShots - timerDecreaseByLevel * PlayerStats.GetCurrentStage();
        if (_minTimeBetweenShots < minTimeLowerLimit) _minTimeBetweenShots = minTimeLowerLimit;
        _maxTimeBetweenShots = startingMaxTimeBetweenShots - timerDecreaseByLevel * PlayerStats.GetCurrentStage();
        if (_maxTimeBetweenShots < maxTimeLowerLimit) _maxTimeBetweenShots = maxTimeLowerLimit;
        
        EnemyLaser.SetEnemySoundController(enemySoundController);
    }

    // Start is called before the first frame update
    public void Start()
    {
        _timeSinceLastLaser = 0.0f;
        _timeTillNextLaser = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
    }

    // Update is called once per frame
    public void Update()
    {
        _timeSinceLastLaser += Time.deltaTime;
        if (!_doingBigAttack && _timeSinceLastLaser > _timeTillNextLaser)
        {
            // 10% chance of doing a big attack
            if (bigAttacks.Length > 0 && Random.Range(1, 11) > 9)
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
            OnAttack?.Invoke();
            ResetTimer();
        }
    }

    private void SingleAimedLaser()
    {
        _currLaser = Random.Range(1, 101) > specialLaserChance
            ? specialLaserPool.GetPooledObject()
            : laserPool.GetPooledObject();
        _laserLogic = _currLaser.GetComponent<EnemyLaser>();
        _laserLogic.Init(_damage, target.transform);
        _currLaser.transform.position = transform.position;
        _currLaser.SetActive(true);
    }

    private void FinishedBigAttack()
    {
        _doingBigAttack = false;
        bigAttacks[_bigAttackIndex].OnFinish -= FinishedBigAttack;
    }

    private void ResetTimer()
    {
        _timeSinceLastLaser = 0.0f;
        _timeTillNextLaser = Random.Range(_minTimeBetweenShots, _maxTimeBetweenShots);
    }
}
