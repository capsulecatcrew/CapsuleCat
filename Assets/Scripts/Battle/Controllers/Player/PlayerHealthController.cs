using Battle.Hitboxes;
using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        private BattleStat _playerHealth;
        [SerializeField] private HealthKillable healthKillable;

        [SerializeField] private BattleController battleController;

        public void Awake()
        {
            PlayerStats.InitHealthKillable(healthKillable);
        }
        
        // oops I started with it
        // have fun I guess
    }
}