using Player.Stats.Persistent;

namespace Battle.Hitboxes
{
    public class Killable : Hitbox
    {
        protected BattleStat BattleStat;

        public delegate void Death();
        public event Death OnDeath;

        public virtual void Awake()
        {
            BattleStat.OnStatDeplete += Die;
        }

        public virtual void Init(UpgradeableLinearStat maxStat, params Firer[] enemies)
        {
            base.Init(enemies);
            maxStat.SetLevel(PlayerStats.GetCurrentStage());
            BattleStat = maxStat.CreateBattleStat();
        }

        public override bool Hit(Firer firer, float damage, bool ignoreIFrames)
        {
            return base.Hit(firer, damage, ignoreIFrames) && BattleStat.MinusValue(damage, ignoreIFrames);
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