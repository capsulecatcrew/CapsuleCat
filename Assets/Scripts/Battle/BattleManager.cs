using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private EnemyStats enemy;
    [SerializeField] private GameObject enemyBody;
    [SerializeField] private GameObject[] enemyShields;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject[] playerAbsorbers;

    public delegate void PlayerWin();
    public delegate void PlayerLose();
    
    public event PlayerWin OnPlayerWin;

    public event PlayerLose OnPlayerLose;
    
    public void Start()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
        var damageable = playerBody.GetComponent<Damageable>();
        PlayerStats.UpdateDamageable(damageable);
        enemy.GetHealthStat().OnDeath += Win;
        PlayerStats.GetHealthStat().OnDeath += Lose;
        enemy.SetEnemyColor(enemyBody);
    }

    public void RegisterBullet(Bullet bullet)
    {
        bullet.OnBulletHitUpdate += OnHitUpdate;
    }

    public void RegisterLaser(EnemyLaser laser)
    {
        laser.OnLaserHitUpdate += OnHitUpdate;
    }

    public void DeregisterBullet(Bullet bullet)
    {
        bullet.OnBulletHitUpdate -= OnHitUpdate;
    }
    
    public void DeregisterLaser(EnemyLaser laser)
    {
        laser.OnLaserHitUpdate -= OnHitUpdate;
    }

    private void OnHitUpdate(GameObject hitObject, float damage, bool ignoreIFrames)
    {
        if (hitObject == enemyBody)
        {
            enemy.TakeDamage(damage, ignoreIFrames);
            return;
        }
        
        if (hitObject == playerBody)
        {
            PlayerStats.Damage(damage, ignoreIFrames);
            return;
        }
        
        hitObject.GetComponent<Damageable>().TakeDamage(damage, ignoreIFrames);
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
