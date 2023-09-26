using Battle;
using Battle.Hitboxes;
using Player.Stats.Persistent;
using UnityEngine;

namespace Enemy
{
    public class EnemyController: MonoBehaviour
    {
        private static readonly UpgradeableLinearStat BodyMainMaxHealth = new("", int.MaxValue, 100, 20, 0, 0, true);
        [SerializeField] private HealthKillable bodyMain;
        
        private static readonly UpgradeableLinearStat BodyPartsMaxHealth = new("", int.MaxValue, 30, 5, 0, 0, true);
        [SerializeField] private HealthKillable[] bodyParts;

        [SerializeField] private BattleController battleController;
        
        public delegate void EnemyMainDamaged(float amount);
        public event EnemyMainDamaged OnEnemyMainDamaged;

        public void Awake()
        {
            InitMaxHealths();
            SetEnemyColor();
            bodyMain.OnDamaged += HandleEnemyDamaged;
            bodyMain.OnDeath += HandleEnemyDeath;
        }

        private void InitMaxHealths()
        {
            InitBodyMainMaxHealth();
            BodyPartsMaxHealth.SetLevel(PlayerStats.GetCurrentStage());
            var healthBarCount = bodyParts.Length;
            for (var i = 0; i < healthBarCount; i++)
            {
                bodyParts[i].Init(BodyPartsMaxHealth, Firer.Player1, Firer.Player2);
            }
        }

        private void InitBodyMainMaxHealth()
        {
            BodyMainMaxHealth.SetLevel(PlayerStats.GetCurrentStage());
            bodyMain.Init(BodyMainMaxHealth, Firer.Player1, Firer.Player2);
        }

        private void HandleEnemyDamaged(float amount)
        {
            OnEnemyMainDamaged?.Invoke(amount);
        }

        private void HandleEnemyDeath()
        {
            battleController.Win();
        }

        private void OnDisable()
        {
            bodyMain.OnDeath -= HandleEnemyDeath;
        }
        
        private void SetEnemyColor()
        {
            var r = Random.Range(0.1f, 1.0f);
            var g = Random.Range(0.1f, 1.0f);
            var b = Random.Range(0.1f, 1.0f);
            gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b);
        }
    }
}