using System.Linq;
using UnityEngine;

namespace Battle
{
    public abstract class Hitbox : MonoBehaviour
    {
        private Firer[] _enemies;

        public delegate void Damaged(float damage);
        public event Damaged OnDamaged;
        
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip damageSound;

        public void Init(Firer[] enemies)
        {
            _enemies = enemies;
        }

        public virtual bool Hit(Firer firer, float damage, bool ignoreIFrames)
        {
            if (!_enemies.Contains(firer)) return false;
            OnDamaged?.Invoke(damage);
            audioSource.PlayOneShot(damageSound);
            return true;
        }
    }
}