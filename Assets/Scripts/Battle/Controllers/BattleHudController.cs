using Battle.Controllers.Player;
using Enemy;
using Player.Stats;
using UnityEngine;

public class BattleHudController : MonoBehaviour
{
    [Header("Entity Managers")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private EnemyController enemyController;

    [Header("Progress Bars")]
    [SerializeField] private ProgressBar enemyHpBar;
    [SerializeField] private ProgressBar playerHpBar;
    [SerializeField] private ProgressBar p1EnergyBar, p2EnergyBar;
    [SerializeField] private ProgressBar p1SpecialBar, p2SpecialBar;

    public void OnEnable()
    {
        SubscribeToAllControllerEvents();
    }
    
    public void Start()
    {
        InitBars();
    }

    public void OnDisable()
    {
        UnsubscribeFromAllControllerEvents();
    }
    
    private void InitBars()
    {
        PlayerStats.InitPlayerHealthBarMax(playerHpBar);
        PlayerStats.InitPlayerEnergyBarMax(1, p1EnergyBar);
        PlayerStats.InitPlayerEnergyBarMax(2, p2EnergyBar);
        PlayerStats.InitPlayerSpecialBarMax(1, p1SpecialBar);
        PlayerStats.InitPlayerSpecialBarMax(2, p2SpecialBar);
        playerController.InitBars(playerHpBar, p1EnergyBar, p2EnergyBar, p1SpecialBar, p2SpecialBar);

        EnemyController.InitEnemyHealthBar(enemyHpBar);
    }

    private void SubscribeToAllControllerEvents()
    {
        // from player manager
        playerController.OnHealthChange += playerHpBar.HandleStatChange;
        playerController.OnP1EnergyChange += p1EnergyBar.HandleStatChange;
        playerController.OnP2EnergyChange += p2EnergyBar.HandleStatChange;
        playerController.OnP1SpecialChange += p1SpecialBar.HandleStatChange;
        playerController.OnP2SpecialChange += p2SpecialBar.HandleStatChange;

        // from enemy manager
        enemyController.OnEnemyPrimaryHealthChanged += enemyHpBar.HandleStatChange;
    }

    private void UnsubscribeFromAllControllerEvents()
    {
        // from player manager
        playerController.OnHealthChange -= playerHpBar.HandleStatChange;
        playerController.OnP1EnergyChange -= p1EnergyBar.HandleStatChange;
        playerController.OnP2EnergyChange -= p2EnergyBar.HandleStatChange;
        playerController.OnP1SpecialChange -= p1SpecialBar.HandleStatChange;
        playerController.OnP2SpecialChange -= p2SpecialBar.HandleStatChange;

        // from enemy manager
        enemyController.OnEnemyPrimaryHealthChanged -= enemyHpBar.HandleStatChange;
    }
}