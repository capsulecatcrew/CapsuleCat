using System.Collections.Generic;
using System.Linq;
using Enemy.Attacks;
using UnityEngine;

namespace Enemy.Bosses.Attacks.Lasers
{
    public class SmugCubeLaserController : BasicLaserController
    {
        [SerializeField] private Transform origin;
        [SerializeField] private int[] laserPatternWeights;
        private List<SmugCubeLaserPattern> _laserPatterns;
        private int _weightSum;

        public new void Start()
        {
            base.Start();
            _laserPatterns = new List<SmugCubeLaserPattern>();
            _weightSum = laserPatternWeights.Aggregate((x, y) => x + y);
            _laserPatterns.Add(new SimpleLaserTrackingPattern(origin, target, Damage, this));
            _laserPatterns.Add(new SimpleLaserRadialPattern(origin, target, Damage, this));
        }

        protected override float SpawnLasers()
        {
            var cooldown = GetLaserPattern().SpawnLasers();
            base.SpawnLasers();
            return cooldown;
        }

        private SmugCubeLaserPattern GetLaserPattern()
        {
            var chosenInt = Random.Range(1, _weightSum + 1);
            var index = 0;
            while (chosenInt > 0)
            {
                chosenInt -= laserPatternWeights[index];
                if (chosenInt <= 0) break;
                ++index;
            }
            return _laserPatterns[index];
        }
    }
}