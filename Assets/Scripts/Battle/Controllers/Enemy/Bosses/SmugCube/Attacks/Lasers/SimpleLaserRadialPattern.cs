using System;
using System.Threading.Tasks;
using Player.Stats;
using Player.Stats.Persistent;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Bosses.Attacks.Lasers
{
    public class SimpleLaserRadialPattern : SmugCubeLaserPattern
    {
        private readonly IntervalLinearStat _laserCountStat = new ("Count", 4, 4, 1, 0, 0, 5, false);
        private EnemyLaser[] _lasersInUse;
        private const float RotationSpeed = 50;
        private int _spinDir = 1;

        public SimpleLaserRadialPattern(
            Transform origin, Transform target, float damage, SmugCubeLaserController controller) :
            base(origin, target, damage, controller)
        {
            _laserCountStat.SetLevel(PlayerStats.GetCurrentStage());
        }

        public override float SpawnLasers()
        {
            var count = (int) _laserCountStat.GetValue();
            var angle =  2 * (float) Math.PI / count;
            var height = Target.transform.position.y;
            _lasersInUse = new EnemyLaser[count];
            _spinDir = Random.Range(0, 2) == 0 ? -1 : 1;
            for (var i = 0; i < count; i++)
            {
                var laser = Controller.GetPooledLaser();
                var dir = new Vector3(30 * Mathf.Sin(angle * i), height, 30 * Mathf.Cos(angle * i));
                laser.Init(Damage, dir);
                laser.gameObject.SetActive(true);
                _lasersInUse[i] = laser;
            }
            Controller.OnTimeUpdate += HandleTimeUpdate;
            StopTimeUpdates();
            return 6.4f;
        }

        private void HandleTimeUpdate(float deltaTime)
        {
            foreach (var laser in _lasersInUse)
            {
                laser.transform.Rotate(new Vector3(0, 1, 0) * (RotationSpeed * _spinDir * deltaTime), Space.World);
            }
        }
        
        private async void StopTimeUpdates()
        {
            await Task.Delay(6400);
            Controller.OnTimeUpdate -= HandleTimeUpdate;
        }
    }
}