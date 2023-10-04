using System.Linq;
using Player.Stats.Persistent;
using UnityEngine;
using UnityEngine.Serialization;

namespace Battle.Hitboxes
{
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private Firer[] takeDamageFrom;
        [SerializeField] private DamageType[] weaknesses;
        [SerializeField] private DamageType[] resistances;

        [SerializeField] private float weaknessMult = 2;
        [SerializeField] private float resistanceMult = 0.5f;

        private const float HitIFrameTime = 1;
        private float _hitIFrameTimer;
        protected bool OnCooldown;
        
        private bool _isShielded;
        
        /// <summary>
        /// Provides incoming damage value.
        /// <p> Do not use for health bars. </p>
        /// </summary>
        public delegate void HitBox(float damage, DamageType damageType);
        public event HitBox OnHitBox;
        
        [SerializeField] protected AudioSource audioSource;
        [SerializeField] protected AudioSource audioSource2;
        [SerializeField] protected AudioClip hitSound;
        private BattleStat _energySoundScalarStat;
        private BattleStat _specialSoundScalarStat;

        private void Update()
        {
            UpdateHitIFrameTimer();
        }

        private void UpdateHitIFrameTimer()
        {
            if (_hitIFrameTimer <= 0) return;
            _hitIFrameTimer -= Time.deltaTime;
        }

        private void ResetHitTimer()
        {
            _hitIFrameTimer = HitIFrameTime;
        }

        /// <summary>
        /// Processes hitbox activation.
        /// <p> Returns false if firer does not match, true otherwise. </p>
        /// <p> Applies damage if returns true AND one of the following: </p>
        /// <p> 1. ignoreIFrames == true </p>
        /// <p> 2. IFrame timer has expired </p>
        /// <p> Damage applied has weakness and resistance multiplier applied. </p>
        /// </summary>
        /// <param name="firer">Firer of the attack.</param>
        /// <param name="damage">Damage of the attack.</param>
        /// <param name="damageType">Damage type of the attack.</param>
        /// <param name="ignoreIFrames">Whether to ignore IFrame timer.</param>
        /// <returns>Whether hitbox has been hit.</returns>
        public virtual bool Hit(Firer firer, float damage, DamageType damageType = DamageType.Normal, bool ignoreIFrames = false)
        {
            if (_isShielded) return false;
            if (!takeDamageFrom.Contains(firer)) return false;
            if (!ignoreIFrames && _hitIFrameTimer > 0)
            {
                OnCooldown = true;
                return true;
            }
            if (weaknesses.Contains(damageType)) damage *= weaknessMult;
            if (resistances.Contains(damageType)) damage *= resistanceMult;
            OnHitBox?.Invoke(damage, damageType);
            ResetHitTimer();
            if (damageType == DamageType.Normal)
            {
                if (_energySoundScalarStat != null) audioSource.pitch = _energySoundScalarStat.GetStatPercentage() * 0.75f + 1;
                audioSource.PlayOneShot(hitSound);
            }
            else
            {
                if (_specialSoundScalarStat != null) audioSource2.pitch = _specialSoundScalarStat.GetStatPercentage() * 0.75f + 1;
                if (audioSource2 != null)
                {
                    audioSource2.PlayOneShot(hitSound);
                }
                else
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
            return true;
        }

        public void EnableShield()
        {
            _isShielded = true;
        }

        public void DisableShield()
        {
            _isShielded = false;
        }

        /// <summary>
        /// Registers a battle stat to scale the energy absorber hit sound pitch by.
        /// New value = GetStatPercentage() * 0.75f + 1
        /// </summary>
        public void RegisterEnergySoundScalarStat(BattleStat battleStat)
        {
            _energySoundScalarStat = battleStat;
        }
        
        /// <summary>
        /// Registers a battle stat to scale the special absorber hit sound pitch by.
        /// New value = GetStatPercentage() * 0.75f + 1
        /// </summary>
        public void RegisterSpecialSoundScalarStat(BattleStat battleStat)
        {
            _specialSoundScalarStat = battleStat;
        }
    }
}