using Battle.Enemy;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;

namespace Enemy.Attacks
{
    public abstract class BasicBulletController : MonoBehaviour
    {
        [SerializeField] protected EnemyBulletPool bulletPool;
        [SerializeField] protected float bulletSpeed;
        [SerializeField] protected int[] bulletAmounts;
        
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
        private IntervalLinearStat _damageStat;
        protected float Damage;
        
        [Header("Audio")]
        [SerializeField] protected EnemySoundController enemySoundController;
        
        public delegate void Attack();
        public event Attack OnAttack;

        public virtual void Start()
        {
            CreateStats();
            Damage = _damageStat.GetValue();
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
            _damageStat = new IntervalLinearStat("Damage", int.MaxValue,
                baseDamage,
                damageIncrease, 0, 0,
                damageUpgradeInterval, false);
            _minCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
            _maxCooldownTime.SetLevel(PlayerStats.GetCurrentStage());
            _damageStat.SetLevel(PlayerStats.GetCurrentStage());
            _cooldownTime = new RandomStat(_minCooldownTime, _maxCooldownTime);
        }
        
        public void Update()
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer > float.Epsilon) return;
            _cooldownTimer = SpawnBullets() + _cooldownTime.GenerateRandomValue();
        }

        protected virtual float SpawnBullets()
        {
            enemySoundController.PlayBulletSound();
            OnAttack?.Invoke();
            return 0;
        }
    }
}