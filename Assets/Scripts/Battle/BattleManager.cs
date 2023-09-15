using HUD.ProgressBars;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Battle scene manager class.
/// <p>Steps for update functions:</p>
/// <p>1. Update UI bars</p>
/// <p>2. Take any needed gameplay actions</p>
/// <p>3. Play sound</p>
/// </summary>
public class BattleManager : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private EnemyStats enemy;

    public delegate void PlayerWin();
    public delegate void PlayerLose();
    
    public event PlayerWin OnPlayerWin;

    public event PlayerLose OnPlayerLose;
    
    public void Start()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
        enemy.GetHealthStat().OnDeath += Win;
        PlayerStats.GetHealthStat().OnDeath += Lose;
    }
    
    private void OnDisable()
    {
        GlobalAudio.Singleton.StopMusic();
    
    }

    void Win()
    {
        GlobalAudio.Singleton.PlaySound("Explode");
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Victory");
        levelLoader.LoadLevel("Victory");
        OnPlayerWin?.Invoke();
    }

    void Lose()
    {
        GlobalAudio.Singleton.StopMusic();
        levelLoader.LoadLevel("Game Over");
        OnPlayerLose?.Invoke();
    }
}
