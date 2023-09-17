using System.Linq;
using Battle;
using Player.Stats.Persistent;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    private const float DamageCooldown = 0.5f;
    
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private GameObject enemyBody;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject[] player1Absorbers;
    [SerializeField] private GameObject[] player2Absorbers;

    private BattleStat _playerHealth;
    [SerializeField] private ProgressBar playerHealthBar;
    
    private BattleStat _player1Energy;
    private BattleStat _player2Energy;
    [SerializeField] private ProgressBar player1EnergyBar;
    [SerializeField] private ProgressBar player2EnergyBar;
    [SerializeField] private WingGlow player1WingGlow;
    [SerializeField] private WingGlow player2WingGlow;
    
    private BattleStat _player1Special;
    private BattleStat _player2Special;
    [SerializeField] private ProgressBar player1SpecialBar;
    [SerializeField] private ProgressBar player2SpecialBar;

    private static readonly UpgradeableLinearStat EnemyMaxHealth = new ("Enemy Max Health", int.MaxValue, 50, 20, 0, 0);
    private BattleStat _enemyHealth;
    [SerializeField] private ProgressBar enemyHealthBar;

    [SerializeField] private EnemyShieldController enemyShieldController;

    public delegate void PlayerWin();
    public event PlayerWin OnPlayerWin;
    
    public delegate void PlayerLose();
    public event PlayerLose OnPlayerLose;

    public delegate void EnemyHit(float damage);
    public event EnemyHit OnEnemyHit;
    
    public delegate void EnemyShieldHit(float damage);
    public event EnemyShieldHit OnEnemyShieldHit;

    public delegate void PlayerHit(float damage);
    public event PlayerHit OnPlayerHit;

    public delegate void PlayerAbsorberHit(float damage, int playerNum);
    public event PlayerAbsorberHit OnPlayerAbsorberHit;

    public delegate void TimeChanged(float deltaTime);
    public event TimeChanged OnTimeChanged;
    
    public void Start()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
        if (PlayerStats.GetCurrentStage() != 1) EnemyMaxHealth.UpgradeLevel();

        _playerHealth = PlayerStats.CreateBattleHealthStat(playerHealthBar);
        _playerHealth.SetMaxChangeCooldown(DamageCooldown);
        
        _player1Energy = PlayerStats.CreateBattleEnergyStat(1, player1EnergyBar);
        _player1Special = PlayerStats.CreateBattleSpecialStat(1, player1SpecialBar);

        _player2Energy = PlayerStats.CreateBattleEnergyStat(2, player2EnergyBar);
        _player2Special = PlayerStats.CreateBattleSpecialStat(2, player2SpecialBar);
        
        _enemyHealth = EnemyMaxHealth.CreateBattleStat();
        _enemyHealth.SetMaxChangeCooldown(DamageCooldown);
        
        enemyHealthBar.SetMaxValue(0, EnemyMaxHealth.Value, 0);
        _enemyHealth.OnStatChange += enemyHealthBar.SetValue;
        enemyHealthBar.SetValue(_enemyHealth.Value);

        SetEnemyColor();

        _playerHealth.OnStatDeplete += Lose;
        _enemyHealth.OnStatDeplete += Win;

        OnPlayerWin += PlayerStats.Win;
        OnPlayerLose += PlayerStats.Lose;
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        _playerHealth.UpdateCooldown(deltaTime);
        _enemyHealth.UpdateCooldown(deltaTime);
        _player1Energy.UpdateCooldown(deltaTime);
        _player2Energy.UpdateCooldown(deltaTime);
        _player1Special.UpdateCooldown(deltaTime);
        _player2Special.UpdateCooldown(deltaTime);
        OnTimeChanged?.Invoke(deltaTime);
    }

    private void SetEnemyColor()
    {
        var r = Random.Range(0.1f, 1.0f);
        var g = Random.Range(0.1f, 1.0f);
        var b = Random.Range(0.1f, 1.0f);
        enemyBody.GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    public bool HitTarget(Firer firer, GameObject hitObject, float damage, bool ignoreIFrames)
    {
        switch (firer)
        {
            case Firer.Player1 when hitObject == enemyBody:
                if (_enemyHealth.MinusValue(damage, ignoreIFrames)) OnEnemyHit?.Invoke(damage);
                _player1Special.AddValue(PlayerStats.ApplySpecialDamageMultipler(1, damage), false);
                return true;
            case Firer.Player2 when hitObject == enemyBody:
                if (_enemyHealth.MinusValue(damage, ignoreIFrames)) OnEnemyHit?.Invoke(damage);
                _player2Special.AddValue(PlayerStats.ApplySpecialDamageMultipler(2, damage), false);
                return true;
            case Firer.Player1 when enemyShieldController.IsEnemyShield(hitObject):
                if (enemyShieldController.HitEnemyShield(hitObject, damage, ignoreIFrames)) OnEnemyShieldHit?.Invoke(damage);
                return true;
            case Firer.Player2 when enemyShieldController.IsEnemyShield(hitObject):
                if (enemyShieldController.HitEnemyShield(hitObject, damage, ignoreIFrames)) OnEnemyShieldHit?.Invoke(damage);
                return true;
            case Firer.Enemy when hitObject == playerBody:
                if (_playerHealth.MinusValue(damage, ignoreIFrames)) OnPlayerHit?.Invoke(damage);
                _player1Special.AddValue(PlayerStats.ApplySpecialDamagedMultipler(1, damage), false);
                _player2Special.AddValue(PlayerStats.ApplySpecialDamagedMultipler(2, damage), false);
                return true;
            case Firer.Enemy when player1Absorbers.Contains(hitObject):
                var absorbedEnergy1 = PlayerStats.ApplyEnergyAbsorbMultiplier(1, damage);
                if (_player1Energy.AddValue(absorbedEnergy1, false))
                {
                    OnPlayerAbsorberHit?.Invoke(damage, 1);
                    player1WingGlow.TurnOnGlow(0);
                }
                _player1Special.AddValue(PlayerStats.ApplySpecialAbsorbMultipler(1, absorbedEnergy1), false);
                return true;
            case Firer.Enemy when player2Absorbers.Contains(hitObject):
                var absorbedEnergy2 = PlayerStats.ApplyEnergyAbsorbMultiplier(2, damage);
                if (_player2Energy.AddValue(absorbedEnergy2, false))
                {
                    OnPlayerAbsorberHit?.Invoke(damage, 2);
                    player2WingGlow.TurnOnGlow(0);
                }
                _player2Special.AddValue(PlayerStats.ApplySpecialAbsorbMultipler(2, absorbedEnergy2), false);
                return true;
            default:
                return false;
        }
    }

    private void OnDisable()
    {
        GlobalAudio.Singleton.StopMusic();
    }

    private void Win()
    {
        GlobalAudio.Singleton.PlaySound("Explode");
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Victory");
        levelLoader.LoadLevel("Victory");
        OnPlayerWin?.Invoke();
    }

    private void Lose()
    {
        GlobalAudio.Singleton.StopMusic();
        levelLoader.LoadLevel("Game Over");
        EnemyMaxHealth.Reset();
        OnPlayerLose?.Invoke();
    }

    public bool HasEnergy(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => _player1Energy.CanMinusValue(amount),
            2 => _player2Energy.CanMinusValue(amount),
            _ => false
        };
    }

    public void UseEnergy(int playerNum, float amount)
    {
        switch (playerNum)
        {
            case 1:
                _player1Energy.MinusValue(amount, false);
                break;
            case 2:
                _player2Energy.MinusValue(amount, false);
                break;
        }
    }

    public bool HasSpecial(int playerNum, float amount)
    {
        return playerNum switch
        {
            1 => _player1Special.CanMinusValue(amount),
            2 => _player2Special.CanMinusValue(amount),
            _ => false
        };
    }

    public void UseSpecial(int playerNum, float amount)
    {
        switch (playerNum)
        {
            case 1:
                _player1Special.MinusValue(amount, true);
                break;
            case 2:
                _player2Special.MinusValue(amount, true);
                break;
        }
    }

    public void HealPlayer(float amount)
    {
        _playerHealth.AddValue(amount, true);
    }
}
