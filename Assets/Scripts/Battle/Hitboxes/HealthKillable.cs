using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Hitboxes
{
    public class HealthKillable : Killable
    {
        [SerializeField] private ProgressBar healthBar;
        
        public override void Init(UpgradeableLinearStat maxStat, params Firer[] enemies)
        {
            base.Init(maxStat, enemies);
            OnDamaged += HandleDamageBarUpdate;
        }

        public override void OnDisable()
        {
            OnDamaged -= HandleDamageBarUpdate;
        }

        private void HandleDamageBarUpdate(float amount, DamageType unused)
        {
            healthBar.ChangeValueBy(-amount);
        }
    }
}