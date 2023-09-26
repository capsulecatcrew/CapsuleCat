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
            OnDamaged += healthBar.SetValue;
        }

        public override void OnDisable()
        {
            OnDamaged -= healthBar.SetValue;
        }
    }
}