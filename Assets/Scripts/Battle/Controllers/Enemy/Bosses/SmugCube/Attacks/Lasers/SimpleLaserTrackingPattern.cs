using UnityEngine;

namespace Enemy.Bosses.Attacks.Lasers
{
    public class SimpleLaserTrackingPattern : SmugCubeLaserPattern
    {
        public SimpleLaserTrackingPattern(
            Transform origin, Transform target, float damage, SmugCubeLaserController controller) :
            base(origin, target, damage, controller) { }

        public override float SpawnLasers()
        {
            var laser = Controller.GetPooledLaser();
            laser.Init(Damage, Target.transform, 2.02f, true);
            laser.gameObject.SetActive(true);
            return 3.4f;
        }
    }
}