using Battle.Controllers.Player;
using Enemy;
using Player.Special;
using Player.Stats;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSoundController playerSoundController;
    [SerializeField] private EnemyController enemyController;

    [SerializeField] private LevelLoader levelLoader;

    private bool _hasLost;

    public delegate void PlayerWin();
    public event PlayerWin OnPlayerWin;
    
    public delegate void PlayerLose();
    public event PlayerLose OnPlayerLose;

    public void Awake()
    {
        PlayerStats.UpdatePlayerController(playerController, playerSoundController);
    }

    public void Start()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
    }

    private void OnEnable()
    {
        enemyController.OnEnemyDeath += Win;
        playerController.OnPlayerDeath += Lose;
        OnPlayerWin += PlayerStats.Win;
        OnPlayerLose += PlayerStats.Lose;
    }

    private void OnDisable()
    {
        enemyController.OnEnemyDeath -= Win;
        playerController.OnPlayerDeath -= Lose;
        OnPlayerWin -= PlayerStats.Win;
        OnPlayerLose -= PlayerStats.Lose;
        GlobalAudio.Singleton.StopMusic();
    }

    private void Win()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlaySound("ENEMY_DEATH");
        GlobalAudio.Singleton.PlayMusic("Victory");
        levelLoader.LoadLevel("Victory");
        OnPlayerWin?.Invoke();
    }

    private void Lose()
    {
        if (_hasLost) return;
        _hasLost = true;
        GlobalAudio.Singleton.StopMusic();
        OnPlayerLose?.Invoke();
        levelLoader.LoadLevel("Game Over");
    }
}
