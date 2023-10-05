using System.Collections.Generic;
using System.Linq;
using Battle.Hitboxes;
using Player.Stats;
using Player.Stats.Persistent;
using Player.Stats.Templates;
using UnityEngine;

namespace Enemy
{
    public class EnemyController: MonoBehaviour
    {
        [System.Serializable]
        private class PrimaryPart
        {
            public Killable hitbox;
            public float baseHealth;
            public float healthIncrement;

            public UpgradeableStat MaxHealthStat;
            public static float TotalHealth;

            public static void Init(IEnumerable<PrimaryPart> primaryParts)
            {
                TotalHealth = 0;
                foreach (var part in primaryParts)
                {
                    part.InitPrimaryPartMaxHealth();
                    TotalHealth += part.MaxHealthStat.GetValue();
                }
            }

            private void InitPrimaryPartMaxHealth()
            {
                MaxHealthStat = new UpgradeableLinearStat(
                    "", int.MaxValue, baseHealth, healthIncrement, 0, 0, true);
                MaxHealthStat.SetLevel(PlayerStats.GetCurrentStage());
                hitbox.Init(MaxHealthStat);
            }
        }

        // Body parts that contribute to the main HP of the enemy.
        // The total enemy HP will be the sum of each of these HPs.
        // These must all be destroyed for victory.
        [SerializeField] private PrimaryPart[] primaryParts;
        // body parts that do not contribute to the main HP of the enemy
        [SerializeField] private Killable[] secondaryParts;

        private readonly UpgradeableLinearStat _secondaryPartMaxHealth = new("", int.MaxValue, 30, 5, 0, 0, true);

        private int _primaryPartsLeft;
        
        public delegate void EnemyPrimaryHealthChanged(float amount);
        public event EnemyPrimaryHealthChanged OnEnemyPrimaryHealthChanged;

        public delegate void EnemySecondaryHealthChanged(float amount);
        public event EnemySecondaryHealthChanged OnEnemySecondaryHealthChanged;

        public delegate void EnemyDeath();
        public event EnemyDeath OnEnemyDeath;

        public static void InitEnemyHealthBar(ProgressBar healthBar)
        {
            healthBar.SetMaxValue(PrimaryPart.TotalHealth);
            healthBar.SetValue(PrimaryPart.TotalHealth);
        }

        public void OnEnable()
        {
            InitMaxHealths();
        }
        
        private void InitMaxHealths()
        {
            PrimaryPart.Init(primaryParts);
            InitPrimaryPartMaxHealths();
            InitSecondaryPartMaxHealths();
        }

        private void InitPrimaryPartMaxHealths()
        {
            _primaryPartsLeft = primaryParts.Length;
            foreach (var part in primaryParts)
            {
                part.hitbox.OnHealthChanged += HandleEnemyPrimaryHealthChanged;
                part.hitbox.OnDeath += HandlePrimaryPartDestroyed;
            }
        }

        private void InitSecondaryPartMaxHealths()
        {
            _secondaryPartMaxHealth.SetLevel(PlayerStats.GetCurrentStage());
            // TODO future secondary parts will have health bars attached.
            foreach (var part in secondaryParts)
            {
                part.Init(_secondaryPartMaxHealth);
                part.OnHealthChanged += HandleEnemySecondaryHealthChanged;
            }
        }

        public void OnDisable()
        {
            foreach (var part in primaryParts.Where(part => part.hitbox != null))
            {
                part.hitbox.OnHealthChanged -= HandleEnemyPrimaryHealthChanged;
                part.hitbox.OnDeath -= HandlePrimaryPartDestroyed;
            }
            foreach (var part in secondaryParts)
            {
                part.OnHealthChanged -= HandleEnemySecondaryHealthChanged;
            }
        }

        private void HandleEnemyPrimaryHealthChanged(float amount)
        {
            OnEnemyPrimaryHealthChanged?.Invoke(amount);
        }

        private void HandleEnemySecondaryHealthChanged(float amount)
        {
            OnEnemySecondaryHealthChanged?.Invoke(amount);
        }
        
        private void HandlePrimaryPartDestroyed()
        {
            _primaryPartsLeft--;
            if (_primaryPartsLeft > 0) return;
            HandleEnemyDeath();
        }

        private void HandleEnemyDeath()
        {
            OnEnemyDeath?.Invoke();
        }
    }
}