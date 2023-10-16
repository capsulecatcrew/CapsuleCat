using Player.Stats.Persistent;
using Player.Stats.Templates;
using UnityEngine;

namespace Battle.Hitboxes
{
    public class Killable : Hitbox
    {
        protected BattleStat BattleStat;
        [SerializeField] protected AudioClip deathSound;
        
        public delegate void HealthChanged(float damage);
        /// <summary>
        /// Provides received instead of incoming damage.
        /// <p> Useful for when damage reduces HP to below 0. </p>
        /// </summary>
        public event HealthChanged OnHealthChanged;

        public delegate void Death();
        public event Death OnDeath;

        public void Init(Stat stat)
        {
            BattleStat = stat.CreateBattleStat();
            BattleStat.OnStatChange += HandleStatChange;
            BattleStat.OnStatDeplete += Die;
        }
        
        public void InitHealthBar(ProgressBar healthBar)
        {
            healthBar.SetValue(BattleStat.GetValue());
        }

        public override bool Hit(Firer firer, float damage, DamageType damageType = DamageType.Normal, bool ignoreIFrames = false)
        {
            if (!base.Hit(firer, damage, damageType, ignoreIFrames)) return false;
            if (!IsOnCooldown) return BattleStat.MinusValue(damage);
            IsOnCooldown = false;
            return true;
        }

        private void Die()
        {
            audioSource.PlayOneShot(deathSound);
            OnDeath?.Invoke();
            gameObject.SetActive(false);
        }

        public virtual void OnDisable()
        {
            BattleStat.OnStatChange -= HandleStatChange;
            BattleStat.OnStatDeplete -= Die;
        }

        private void HandleStatChange(float change)
        {
            OnHealthChanged?.Invoke(change);
        }
        
        public void Heal(float amount)
        {
            BattleStat.AddValue(amount);
        }
    }
}