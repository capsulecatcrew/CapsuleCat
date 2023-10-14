using Battle.Enemy;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;

namespace Enemy.Attacks
{
    public abstract class BasicLaserController : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        [SerializeField] private EnemyLaserPool laserPool;
        [SerializeField] private EnemyLaserPool specialLaserPool;
        [SerializeField] private int specialChance = 10;

        [Header("Cooldown Scaling")]
        [SerializeField] private float baseMinCooldown = 6;
        [SerializeField] private float minCooldownReduction = 0.3f;
        [SerializeField] private float baseMaxCooldown = 20;
        [SerializeField] private float maxCooldownReduction = 0.5f;
        [SerializeField] private float absoluteMinCooldown = 1.5f;
        [SerializeField] private float absoluteMaxCooldown = 3.0f;
        private ClampedLinearStat _minCooldownTime;
        private ClampedLinearStat _maxCooldownTime;
        private RandomStat _cooldownTime;
        private float _cooldownTimer;
        
        [Header("Damage Scaling")]
        [SerializeField] private int baseDamage = 4;
        [SerializeField] private int damageIncrease = 1;
        [SerializeField] private int damageUpgradeInterval = 5;
        private IntervalLinearStat _damageStat;
        protected float Damage;
        
        [Header("Audio")]
        [SerializeField] private EnemySoundController enemySoundController;
        
        public delegate void Attack();
        public event Attack OnAttack;
        
        public delegate void TimeUpdate(float deltaTime);
        public event TimeUpdate OnTimeUpdate;
        
        public virtual void Start()
        {
            CreateStats();
            Damage = _damageStat.GetValue();
            _cooldownTimer = _cooldownTime.GenerateRandomValue();
            EnemyLaser.SetEnemySoundController(enemySoundController);
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
                absoluteMaxCooldown, float.MaxValue, false);
            _damageStat = new IntervalLinearStat("Damage", int.MaxValue,
                baseDamage,
                damageIncrease, 0, 0,
                damageUpgradeInterval, false);
            _minCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
            _maxCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
            _damageStat.SetLevel(PlayerStats.GetCurrentStage());
            _cooldownTime = new RandomStat(_minCooldownTime, _maxCooldownTime);
        }
        
        public virtual void Update()
        {
            OnTimeUpdate?.Invoke(Time.deltaTime);
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer > float.Epsilon) return;
            _cooldownTimer = SpawnLasers() + _cooldownTime.GenerateRandomValue();
        }

        protected virtual float SpawnLasers()
        {
            OnAttack?.Invoke();
            return 0;
        }
        
        public EnemyLaser GetPooledLaser()
        {
            return Random.Range(1, 101) <= specialChance
                ? specialLaserPool.GetPooledObject()
                : laserPool.GetPooledObject();
        }
    }
}