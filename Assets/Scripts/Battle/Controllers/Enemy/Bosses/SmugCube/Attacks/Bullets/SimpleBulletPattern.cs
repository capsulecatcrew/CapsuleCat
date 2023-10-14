using System.Collections.Generic;
using System.Threading.Tasks;
using Battle;
using Battle.Enemy;
using UnityEngine;

namespace Enemy.Bosses.Attacks
{
    public class SimpleBulletPattern : SmugCubeBulletPattern
    {
        private const int Delay = 250;

        public SimpleBulletPattern(
            Transform leftOrigin, Transform rightOrigin, EnemyBulletPool bulletPool,
            float damage, float speed, int[] amounts) :
            base(leftOrigin, rightOrigin, bulletPool, damage, speed, amounts) { }

        public override float SpawnBullets()
        {
            var spawnAmount = GetSpawnAmount() / 2;
            for (var i = 0; i < spawnAmount; i++)
            {
                ShootBullet(LeftOrigin, i);
            }
            for (var i = 0; i < spawnAmount; i++)
            {
                ShootBullet(RightOrigin, i);
            }
            return (float) Delay / 1000 * spawnAmount;
        }

        private async void ShootBullet(Transform origin, int number)
        {
            await Task.Delay(Delay * number);
            var spawnPosition = origin.position;
            var bullet = BulletPool.GetPooledObject();
            bullet.gameObject.transform.position = spawnPosition;
            bullet.Init(Damage, Speed, origin.forward, Firer.Enemy);
            bullet.gameObject.SetActive(true);
        }
    }
}