using Battle.Enemy;
using UnityEngine;

namespace Enemy.Bosses.Attacks
{
    public abstract class SmugCubeBulletPattern
    {
        protected readonly Transform LeftOrigin;
        protected readonly Transform RightOrigin;
        protected readonly EnemyBulletPool BulletPool;
        protected readonly float Damage;
        protected readonly float Speed;
        private readonly int[] _amounts;

        protected SmugCubeBulletPattern(
            Transform leftOrigin, Transform rightOrigin, EnemyBulletPool bulletPool,
            float damage, float speed, int[] amounts)
        {
            LeftOrigin = leftOrigin;
            RightOrigin = rightOrigin;
            BulletPool = bulletPool;
            Damage = damage;
            Speed = speed;
            _amounts = amounts;
        }

        public abstract float SpawnBullets();

        protected int GetSpawnAmount()
        {
            return _amounts[Random.Range(0, _amounts.Length)];
        }
    }
}