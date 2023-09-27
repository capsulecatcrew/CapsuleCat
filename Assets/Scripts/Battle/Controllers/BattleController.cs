using Battle.Controllers.Player;
using Enemy;
using Player.Special;
using Player.Stats.Persistent;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleController : MonoBehaviour
{
    // TODO Implement ramping using this in EnemyBulletSpray
    // private static readonly UpgradeableLinearStat EnemyBulletDamage =
    //     new ("Enemy Max Health", int.MaxValue, 2, 20, 0, 0, true);
    // TODO Implement ramping using this in EnemyLaser
    // private static readonly UpgradeableLinearStat EnemyLaserDamage =
    //     new ("Enemy Max Health", int.MaxValue, 2, 20, 0, 0, true);
    
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private EnemyShieldsController enemyShieldsController;

    [SerializeField] private LevelLoader levelLoader;

    private bool _hasLost;

    public delegate void PlayerWin();
    public event PlayerWin OnPlayerWin;
    
    public delegate void PlayerLose();
    public event PlayerLose OnPlayerLose;
    
    // public delegate void PlayerShotFired();
    // public event PlayerShotFired OnPlayerShotFired;

    public void Awake()
    {
        // _playerHealth = PlayerStats.InitHealthKillable(playerHealthBar);
        // _playerHealth.OnStatDecrease += PlayPlayerHurtSound;
        //
        // _player1Energy = PlayerStats.CreateBattleEnergyStat(1, player1EnergyBar);
        // _player1Energy.OnStatIncrease += PlayEnergyAbsorbSound;
        // _player1Special = PlayerStats.CreateBattleSpecialStat(1, player1SpecialBar);
        //
        // _player2Energy = PlayerStats.CreateBattleEnergyStat(2, player2EnergyBar);
        // _player2Energy.OnStatIncrease += PlayEnergyAbsorbSound;
        // _player2Special = PlayerStats.CreateBattleSpecialStat(2, player2SpecialBar);

        // _playerHealth.OnStatDeplete += Lose;

        // OnPlayerWin += PlayerStats.Win;
        // OnPlayerLose += PlayerStats.Lose;
        PlayerStats.SetSpecialMove(1, SpecialMoveEnum.MoveHeal);
        PlayerStats.SetSpecialMove(2, SpecialMoveEnum.ShootVampiric);
        PlayerStats.UpdateSpecialMoveBattleManagers(playerController, enemyController);
    }

    public void Start()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        // _playerHealth.UpdateCooldown(deltaTime);
        // _enemyHealth.UpdateCooldown(deltaTime);
        // _player1Energy.UpdateCooldown(deltaTime);
        // _player2Energy.UpdateCooldown(deltaTime);
        // _player1Special.UpdateCooldown(deltaTime);
        // _player2Special.UpdateCooldown(deltaTime);
    }

    // public bool HitTarget(Firer firer, GameObject hitObject, float damage, bool ignoreIFrames)
    // {
    //     switch (firer)
    //     {
    //         case Firer.Player1 when hitObject == enemyBody:
    //             if (_enemyHealth.MinusValue(damage, ignoreIFrames)) OnEnemyHit?.Invoke(damage);
    //             _player1Special.AddValue(PlayerStats.ApplySpecialDamageMultipler(1, damage), true);
    //             return true;
    //         case Firer.Player2 when hitObject == enemyBody:
    //             if (_enemyHealth.MinusValue(damage, ignoreIFrames)) OnEnemyHit?.Invoke(damage);
    //             _player2Special.AddValue(PlayerStats.ApplySpecialDamageMultipler(2, damage), true);
    //             return true;
    //         case Firer.Enemy when hitObject == playerBody:
    //             if (_playerHealth.MinusValue(damage, ignoreIFrames)) OnPlayerHit?.Invoke(damage);
    //             _player1Special.AddValue(PlayerStats.ApplySpecialDamagedMultipler(1, damage), false);
    //             _player2Special.AddValue(PlayerStats.ApplySpecialDamagedMultipler(2, damage), false);
    //             return true;
    //         case Firer.Enemy when player1Absorbers.Contains(hitObject):
    //             var absorbedEnergy1 = PlayerStats.ApplyEnergyAbsorbMultiplier(1, damage);
    //             if (_player1Energy.AddValue(absorbedEnergy1, false))
    //             {
    //                 _player2Energy.AddValue(absorbedEnergy1 * PlayerStats.GetEnergyShare(2), false);
    //                 OnPlayerAbsorberHit?.Invoke(damage, 1);
    //                 player1WingGlow.TurnOnGlow(0);
    //             }
    //             _player1Special.AddValue(PlayerStats.ApplySpecialAbsorbMultipler(1, absorbedEnergy1), false);
    //             return true;
    //         case Firer.Enemy when player2Absorbers.Contains(hitObject):
    //             var absorbedEnergy2 = PlayerStats.ApplyEnergyAbsorbMultiplier(2, damage);
    //             if (_player2Energy.AddValue(absorbedEnergy2, false))
    //             {
    //                 _player1Energy.AddValue(absorbedEnergy2 * PlayerStats.GetEnergyShare(1), false);
    //                 OnPlayerAbsorberHit?.Invoke(damage, 2);
    //                 player2WingGlow.TurnOnGlow(0);
    //             }
    //             _player2Special.AddValue(PlayerStats.ApplySpecialAbsorbMultipler(2, absorbedEnergy2), false);
    //             return true;
    //         default:
    //             return false;
    //     }
    // }

    private void OnEnable()
    {
        enemyController.OnEnemyDefeated += Win;
        playerController.OnPlayerDeath += Lose;
    }

    private void OnDisable()
    {
        enemyController.OnEnemyDefeated -= Win;
        playerController.OnPlayerDeath -= Lose;
        GlobalAudio.Singleton.StopMusic();
    }

    public void Win()
    {
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlaySound("ENEMY_DEATH");
        GlobalAudio.Singleton.PlayMusic("Victory");
        levelLoader.LoadLevel("Victory");
        OnPlayerWin?.Invoke();
    }

    public void Lose()
    {
        if (_hasLost) return;
        _hasLost = true;
        GlobalAudio.Singleton.StopMusic();
        // _playerHealth.OnStatDecrease -= PlayPlayerHurtSound;
        // _player1Energy.OnStatIncrease -= PlayEnergyAbsorbSound;
        // _player2Energy.OnStatIncrease -= PlayEnergyAbsorbSound;
        // _enemyHealth.OnStatDecrease -= PlayEnemyHurtSound;
        OnPlayerLose?.Invoke();
        levelLoader.LoadLevel("Game Over");
    }

    // public void UseSpecial(int playerNum, float amount)
    // {
    //     switch (playerNum)
    //     {
    //         case 1:
    //             _player1Special.MinusValue(amount, true);
    //             break;
    //         case 2:
    //             _player2Special.MinusValue(amount, true);
    //             break;
    //     }
    // }
    //
    // public void HealPlayer(float amount)
    // {
    //     _playerHealth.AddValue(amount, true);
    // }
    
    // public void UpdatePlayerShotFired()
    // {
    //     OnPlayerShotFired?.Invoke();
    // }
    
    // private void PlayEnergyAbsorbSound()
    // {
    //     GlobalAudio.Singleton.PlaySound("PLAYER_ENERGY_ABSORB");
    // }
    //
    // private void PlayPlayerHurtSound()
    // {
    //     GlobalAudio.Singleton.PlaySound("PLAYER_HURT");
    // }
    //
    // private void PlayEnemyHurtSound()
    // {
    //     GlobalAudio.Singleton.PlaySound("ENEMY_HURT");
    // }
}
