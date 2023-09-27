using System.Linq;
using System.Collections.Generic;
using Battle;
using Battle.Hitboxes;
using Player.Stats.Persistent;
using UnityEngine;

namespace Enemy
{
    public class EnemyController: MonoBehaviour
    {
        private const float BaseMaxTotalHealth = 100;
        private const float MaxTotalHealthIncrement = 20;
        private List<UpgradeableLinearStat> _bodyMainMaxHealths = new List<UpgradeableLinearStat>();
        // body parts that contribute to the main HP of the enemy.
        // the total enemy HP will be the sum of each of these HPs.
        [System.Serializable]
        private class PrimaryPart
        {
            public Killable hitbox;
            public int hpWeight = 1; // the proportion of total hp this part should take up
        }
        [SerializeField] private PrimaryPart[] primaryParts;
        
        private UpgradeableLinearStat _bodyPartsMaxHealth = new("", int.MaxValue, 30, 5, 0, 0, true);
        // body parts that do not contribute to the main HP of the enemy
        [SerializeField] private HealthKillable[] secondaryParts;

        private int _primaryPartsLeft;
        
        public delegate void EnemyMainDamaged(float amount);
        public event EnemyMainDamaged OnEnemyMainDamaged;

        public delegate void EnemyDefeated();
        public event EnemyDefeated OnEnemyDefeated;

        public void Awake()
        {
            InitMaxHealths();
            SetEnemyColor();
        }

        private void InitMaxHealths()
        {
            InitBodyMainMaxHealth();
            _bodyPartsMaxHealth.SetLevel(PlayerStats.GetCurrentStage());
            var healthBarCount = secondaryParts.Length;
            for (var i = 0; i < healthBarCount; i++)
            {
                secondaryParts[i].Init(_bodyPartsMaxHealth, Firer.Player1, Firer.Player2);
            }
        }

        private void InitBodyMainMaxHealth()
        {
            int weightsSum = primaryParts.Aggregate(0, (x, y) => x + y.hpWeight);
            weightsSum = weightsSum <= 0 ? 1 : weightsSum;
            
            int numOfPriParts = primaryParts.Length;
            _primaryPartsLeft = numOfPriParts;
            for (int i = 0; i < numOfPriParts; i++)
            {
                float hpProportion = primaryParts[i].hpWeight / (float)weightsSum;
                _bodyMainMaxHealths.Add(new UpgradeableLinearStat("", 
                    int.MaxValue,
                    BaseMaxTotalHealth * hpProportion, 
                    MaxTotalHealthIncrement * hpProportion, 
                    0, 
                    0,
                    true
                    ));
                _bodyMainMaxHealths[i].SetLevel(PlayerStats.GetCurrentStage());

                primaryParts[i].hitbox.Init(_bodyMainMaxHealths[i], Firer.Player1, Firer.Player2);
            }
        }

        private void HandleEnemyDamaged(float amount, DamageType damageType)
        {
            OnEnemyMainDamaged?.Invoke(-amount);
        }

        private void HandleEnemyDeath()
        {
            OnEnemyDefeated?.Invoke();
        }

        private void OnEnable()
        {
            foreach (var part in primaryParts)
            {
                if (part.hitbox == null) continue;
                part.hitbox.OnDamaged += HandleEnemyDamaged;
                part.hitbox.OnDeath += HandlePrimaryPartDestroyed;
            }
        }

        private void OnDisable()
        {
            foreach (var part in primaryParts)
            {
                if (part.hitbox == null) continue;
                part.hitbox.OnDamaged -= HandleEnemyDamaged;
                part.hitbox.OnDeath -= HandlePrimaryPartDestroyed;
            }
        }

        private void HandlePrimaryPartDestroyed()
        {
            _primaryPartsLeft--;
            if (_primaryPartsLeft <= 0)
            {
                HandleEnemyDeath();
            }
        }
        private void SetEnemyColor()
        {
            var r = Random.Range(0.1f, 1.0f);
            var g = Random.Range(0.1f, 1.0f);
            var b = Random.Range(0.1f, 1.0f);
            gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b);
        }

        public float GetMaxHealth()
        {
            return _bodyMainMaxHealths.Aggregate(0f, (x, y) => x + y.GetValue());
        }
        
    }
}