using Battle.Enemy;
using UnityEngine;

namespace Enemy.Bosses.Attacks.Lasers
{
    public abstract class SmugCubeLaserPattern
    {
        protected readonly Transform Origin;
        protected readonly Transform Target;
        protected readonly float Damage;
        protected SmugCubeLaserController Controller;
        
        protected SmugCubeLaserPattern(
            Transform origin, Transform target, float damage, SmugCubeLaserController controller)
        {
            Origin = origin;
            Target = target;
            Damage = damage;
            Controller = controller;
        }

        public abstract float SpawnLasers();
    }
}