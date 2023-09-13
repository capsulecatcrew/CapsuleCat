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
    [Header("Energy")]
    public PlayerEnergy player1Energy;
    public PlayerEnergy player2Energy;

    [Header("UI Bars")]
    public HealthBar playerHealthBar;
    public HealthBar player1EnergyBar;
    public HealthBar player2EnergyBar;
    public HealthBar player1SpecialBar;
    public HealthBar player2SpecialBar;
    public HealthBar enemyHealthBar;

    [Header("Enemy Parameters")]    
    public GameObject enemy;
    public GameObject enemyBody;
    public Damageable enemyDamageable;
    public AudioClip enemyDefeatSound;
    public int enemyBaseHealth = 80;
    public int enemyAddHealth = 20;

    private bool _battleIsOver;
    private void Awake()
    {
        playerDamageable.SetMaxHp((int) PlayerStats.Hp.GetMaxValue());
        playerDamageable.SetCurrentHp((int) PlayerStats.Hp.GetCurrentValue());
        
        var enemyHealth = enemyBaseHealth + PlayerStats.LevelsCompleted * enemyAddHealth;
        enemyDamageable.SetMaxHp(enemyHealth);
        enemyDamageable.SetCurrentHp(enemyHealth);

        enemyBody.GetComponent<Renderer>().material.color = getEnemyColor();
        
        _battleIsOver = false;
    }

    private Color getEnemyColor()
    {
        var r = Random.Range(0.1f, 1.0f);
        var g = Random.Range(0.1f, 1.0f);
        var b = Random.Range(0.1f, 1.0f);
        return new Color(r, g, b);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealthBar.SetMax(playerDamageable.maxHp);
        player1EnergyBar.SetMax(PlayerStats.GetPlayer(1).Energy.GetMaxValue());
        player2EnergyBar.SetMax(PlayerStats.GetPlayer(2).Energy.GetMaxValue());
        enemyHealthBar.SetMax(enemyDamageable.maxHp);
        playerHealthBar.SetFill(playerDamageable.currentHp);
        player1EnergyBar.SetFill(PlayerStats.GetPlayer(1).Energy.GetCurrentValue());
        player2EnergyBar.SetFill(PlayerStats.GetPlayer(2).Energy.GetCurrentValue());
        enemyHealthBar.SetFill(enemyDamageable.currentHp);

        GlobalAudio.Singleton.StopMusic();
        GlobalAudio.Singleton.PlayMusic("Battle " + Random.Range(1, 8));
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        playerDamageable.OnDamage += PlayerDamage;
        playerDamageable.OnDeath += Lose;

        player1Energy.EnergyUpdate += UpdatePlayerEnergyBar;
        player2Energy.EnergyUpdate += UpdatePlayerEnergyBar;

        enemyDamageable.OnDamage += UpdateEnemyHealthBar;
        enemyDamageable.OnDeath += Win;
    }
    private void OnDisable()
    {
        playerDamageable.OnDamage -= PlayerDamage;
        playerDamageable.OnDeath -= Lose;
        
        player1Energy.EnergyUpdate -= UpdatePlayerEnergyBar;
        player2Energy.EnergyUpdate -= UpdatePlayerEnergyBar;
        
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
        player1EnergyBar.SetFill(player1Energy.GetCurrentAmount());
        player2EnergyBar.SetFill(player2Energy.GetCurrentAmount());
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
        PlayerStats.Player1.Money += PlayerStats.LevelsCompleted * 50;
        PlayerStats.Player2.Money += PlayerStats.LevelsCompleted * 50;
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

}
