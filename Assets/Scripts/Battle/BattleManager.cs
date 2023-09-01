using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using Random = UnityEngine.Random;

/// <summary>
/// Battle scene manager class
/// </summary>
public class BattleManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public Damageable playerDamageable;
    public PlayerEnergy playerEnergy;
    
    public GameObject enemyBody;
    public Damageable enemyDamageable;
    public AudioClip enemyDefeatSound;
    
    public GameObject enemy;

    public HealthBar playerHealthBar;
    public HealthBar playerEnergyBar;
    public HealthBar enemyHealthBar;

    private bool _battleIsOver;
    private void Awake()
    {
        playerDamageable.SetMaxHp((int) PlayerStats.Hp.GetMaxValue());
        playerDamageable.SetCurrentHp((int) PlayerStats.Hp.GetCurrentValue());
        int enemyHealth = 30 + PlayerStats.LevelsCompleted * 20;
        enemyDamageable.SetMaxHp(enemyHealth);
        enemyDamageable.SetCurrentHp(enemyHealth);
        
        enemyBody.GetComponent<Renderer>().material.color = new Color(Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f));
        
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
        
        _battleIsOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealthBar.SetMax(playerDamageable.maxHp);
        // playerEnergyBar.SetMax(playerEnergy.GetMax());
        enemyHealthBar.SetMax(enemyDamageable.maxHp);
        playerHealthBar.SetFill(playerDamageable.currentHp);
        // playerEnergyBar.SetFill(playerEnergy.GetCurrentAmount());
        enemyHealthBar.SetFill(enemyDamageable.currentHp);
        SetUpPlayerInputs();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        playerDamageable.OnDamage += PlayerDamage;
        playerDamageable.OnDeath += Lose;

        // playerEnergy.EnergyUpdate += UpdatePlayerEnergyBar;

        enemyDamageable.OnDamage += UpdateEnemyHealthBar;
        enemyDamageable.OnDeath += Win;
    }
    private void OnDisable()
    {
        playerDamageable.OnDamage -= PlayerDamage;
        playerDamageable.OnDeath -= Lose;
        
        // playerEnergy.EnergyUpdate -= UpdatePlayerEnergyBar;
        
        enemyDamageable.OnDamage -= UpdateEnemyHealthBar;
        enemyDamageable.OnDeath -= Win;

    }

    void PlayerDamage(Damageable ignore)
    {
        GlobalAudio.Singleton.PlaySound("Damage");
        playerHealthBar.SetFill(playerDamageable.currentHp);
    }

    void UpdatePlayerEnergyBar()
    {
        playerEnergyBar.SetFill(playerEnergy.GetCurrentAmount());
    }
    void UpdateEnemyHealthBar(Damageable damageable)
    {
        enemyHealthBar.SetFill(enemyDamageable.currentHp);
    }

    void Win(Damageable damageable)
    {
        if (_battleIsOver) return;
        GlobalAudio.Singleton.PlaySound("Explode");
        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Victory");
        enemy.SetActive(false);
        enemyHealthBar.gameObject.SetActive(false);
        playerDamageable.enabled = false;
        PlayerStats.Hp.SetCurrentValue(playerDamageable.currentHp);
        PlayerStats.LevelsCompleted += 1;
        PlayerStats.Money += PlayerStats.LevelsCompleted * 50;
        levelLoader.LoadLevel("Victory");
        _battleIsOver = true;
    }

    void Lose(Damageable damageable)
    {
        if (_battleIsOver) return;
        GlobalAudio.Singleton.StopMusic();
        levelLoader.LoadLevel("Game Over");
        _battleIsOver = true;
    }

    /**
     * =================
     * SetUpPlayerInputs
     * =================
     * Sets the local multiplayer input devices according to how many gamepads/controllers are connected.
     * 
     */
    void SetUpPlayerInputs()
    {
        var player1 = PlayerInput.all[0];
        var player2 = PlayerInput.all[1];
    
        // Discard existing assignments.
        player1.user.UnpairDevices();
        player2.user.UnpairDevices();
    
        // Assign devices and control schemes.
        var gamepadCount = Gamepad.all.Count;
        if (gamepadCount >= 2)
        {
            InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
            InputUser.PerformPairingWithDevice(Gamepad.all[1], user: player2.user);
    
            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("Gamepad");
        }
        else if (gamepadCount == 1)
        {
            InputUser.PerformPairingWithDevice(Gamepad.all[0], user: player1.user);
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
    
            player1.user.ActivateControlScheme("Gamepad");
            player2.user.ActivateControlScheme("SplitKeyboardL");
        }
        else
        {
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player1.user);
            InputUser.PerformPairingWithDevice(Keyboard.current, user: player2.user);
    
            player1.user.ActivateControlScheme(player1.defaultControlScheme);
            player2.user.ActivateControlScheme(player2.defaultControlScheme);
        }

    }
}
