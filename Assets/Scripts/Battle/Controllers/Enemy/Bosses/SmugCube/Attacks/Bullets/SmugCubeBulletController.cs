using System.Collections.Generic;
using System.Linq;
using Enemy.Attacks;
using UnityEngine;

namespace Enemy.Bosses.Attacks
{
    public class SmugCubeBulletController : BasicBulletController
    {
        [SerializeField] private Transform leftOrigin;
        [SerializeField] private Transform rightOrigin;
        [SerializeField] private int patternCount;
        [SerializeField] private int[] bulletPatternWeights;
        private List<SmugCubeBulletPattern> _bulletPatterns;
        private int _weightSum;

        public new void Start()
        {
            base.Start();
            _bulletPatterns = new List<SmugCubeBulletPattern>();
            _weightSum = bulletPatternWeights.Aggregate((x, y) => x + y);
            // Just a proof of concept/test to see if randomisation at least doesn't error lol
            for (var i = 0; i < patternCount; i++)
            {
                _bulletPatterns.Add(
                    new SimpleBulletPattern(leftOrigin, rightOrigin, bulletPool, Damage, bulletSpeed, bulletAmounts));
            }
        }
        
        protected override float SpawnBullets()
        {
            var cooldown = GetBulletPattern().SpawnBullets();
            base.SpawnBullets();
            return cooldown;
        }

        private SmugCubeBulletPattern GetBulletPattern()
        {
            var chosenInt = Random.Range(1, _weightSum + 1);
            var index = 0;
            while (chosenInt > 0)
            {
                chosenInt -= bulletPatternWeights[index];
                if (chosenInt <= 0) break;
                ++index;
            }
            return _bulletPatterns[index];
        }
    }
}