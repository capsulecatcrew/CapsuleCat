using Player.Stats.Persistent;

namespace Battle.Hitboxes
{
    public class Killable : Hitbox
    {
        protected BattleStat BattleStat;

        public delegate void Death();
        public event Death OnDeath;

        public virtual void OnEnable()
        {
            // BattleStat.OnStatDeplete += Die;
        }

        public virtual void Init(UpgradeableLinearStat maxStat, params Firer[] enemies)
        {
            base.Init(enemies);
            maxStat.SetLevel(PlayerStats.GetCurrentStage());
            BattleStat = maxStat.CreateBattleStat();
            BattleStat.OnStatDeplete += Die;
        }

        public override bool Hit(Firer firer, float damage, bool ignoreIFrames = false, DamageType damageType = DamageType.Normal)
        {
            return base.Hit(firer, damage, ignoreIFrames, damageType) && BattleStat.MinusValue(damage);
        }

        private void Die()
        {
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }

        public virtual void OnDisable()
        {
            BattleStat.OnStatDeplete -= Die;
        }
    }
}