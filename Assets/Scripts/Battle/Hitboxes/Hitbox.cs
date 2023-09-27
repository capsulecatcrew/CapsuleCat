using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Battle.Hitboxes
{
    public class Hitbox : MonoBehaviour
    {
        [FormerlySerializedAs("enemies")] [SerializeField] private Firer[] takeDamageFrom;
        [SerializeField] protected float invincibilityTime = 0.5f;
        private float _timeFromLastHit = 0;
        public delegate void Damaged(float damage, DamageType damageType);
        public event Damaged OnDamaged;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip damageSound;

        public void Init(Firer[] enemies)
        {
            takeDamageFrom = enemies;
        }

        private void Update()
        {
            IncrementHitTimer();
        }

        private void IncrementHitTimer()
        {
            if (_timeFromLastHit < invincibilityTime) _timeFromLastHit += Time.deltaTime;
        }

        private void ResetHitTimer()
        {
            _timeFromLastHit = 0.0f;
        }

        public virtual bool Hit(Firer firer, float damage, bool ignoreIFrames = false, DamageType damageType = DamageType.Normal)
        {
            if (!takeDamageFrom.Contains(firer)) return false;
            if (ignoreIFrames || _timeFromLastHit >= invincibilityTime)
            {
                OnDamaged?.Invoke(damage, damageType);
                ResetHitTimer();
            }
            audioSource.PlayOneShot(damageSound);
            return true;
        }
    }
}