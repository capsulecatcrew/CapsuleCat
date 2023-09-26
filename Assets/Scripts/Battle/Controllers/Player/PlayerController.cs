using Player.Stats.Persistent;
using UnityEngine;

namespace Battle.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject playerBody;
        [SerializeField] private GameObject[] player1Absorbers;
        [SerializeField] private GameObject[] player2Absorbers;
        [SerializeField] private GameObject player1ManaShield;
        [SerializeField] private GameObject player2ManaShield;

        private BattleStat _player1Energy;
        private BattleStat _player2Energy;
        [SerializeField] private ProgressBar player1EnergyBar;
        [SerializeField] private ProgressBar player2EnergyBar;
        [SerializeField] private WingGlow player1WingGlow;

        private BattleStat _player1Special;
        private BattleStat _player2Special;
        [SerializeField] private ProgressBar player1SpecialBar;
        [SerializeField] private ProgressBar player2SpecialBar;
        [SerializeField] private WingGlow player2WingGlow;
    }
}