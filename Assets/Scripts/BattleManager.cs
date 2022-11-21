using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BattleManager : MonoBehaviour
{
    public LevelLoader levelLoader;

    public Damageable playerDamageable;
    
    public GameObject enemyBody;
    public Damageable enemyDamageable;
    public AudioClip enemyDefeatSound;
    
    public GameObject enemy;

    public HealthBar playerHealthBar;

    public HealthBar enemyHealthBar;

    private bool _battleIsOver;
    private void Awake()
    {
        playerDamageable.SetMaxHp(PlayerStats.MaxHp);
        playerDamageable.SetCurrentHp(PlayerStats.CurrentHp);
        int enemyHealth = 20 + PlayerStats.LevelsCompleted * 10;
        enemyDamageable.SetMaxHp(enemyHealth);
        enemyDamageable.SetCurrentHp(enemyHealth);
        
        enemyBody.GetComponent<Renderer>().material.color = new Color(Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f));
        
        _battleIsOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerHealthBar.SetMax(playerDamageable.maxHp);
        enemyHealthBar.SetMax(enemyDamageable.maxHp);
        playerHealthBar.SetFill(playerDamageable.currentHp);
        enemyHealthBar.SetFill(enemyDamageable.currentHp);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        playerDamageable.OnDamage += UpdatePlayerHealthBar;
        playerDamageable.OnDeath += Lose;
        enemyDamageable.OnDamage += UpdateEnemyHealthBar;
        enemyDamageable.OnDeath += Win;
    }
    private void OnDisable()
    {
        playerDamageable.OnDamage -= UpdatePlayerHealthBar;
        playerDamageable.OnDeath -= Lose;
        enemyDamageable.OnDamage -= UpdateEnemyHealthBar;
        enemyDamageable.OnDeath -= Win;

    }

    void UpdatePlayerHealthBar(Damageable damageable)
    {
        playerHealthBar.SetFill(playerDamageable.currentHp);
    }
    void UpdateEnemyHealthBar(Damageable damageable)
    {
        enemyHealthBar.SetFill(enemyDamageable.currentHp);
    }

    void Win(Damageable damageable)
    {
        if (_battleIsOver) return;
        GlobalAudio.AudioSource.PlayOneShot(enemyDefeatSound);
        enemy.SetActive(false);
        enemyHealthBar.gameObject.SetActive(false);
        playerDamageable.enabled = false;
        PlayerStats.MaxHp = playerDamageable.maxHp;
        PlayerStats.CurrentHp = playerDamageable.currentHp;
        PlayerStats.LevelsCompleted += 1;
        levelLoader.LoadLevel("Victory");
        _battleIsOver = true;
    }

    void Lose(Damageable damageable)
    {
        if (_battleIsOver) return;
        levelLoader.LoadLevel("Game Over");
        _battleIsOver = true;
    }
}
