using Battle.Controllers.Player;
using Enemy;
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

    void Start()
    {
        InitializeBars();
    }

    void InitializeBars()
    {
        playerHpBar.SetMaxValue(PlayerStats.GetMaxHealth());
        playerHpBar.SetValue(playerController.GetCurrentHealth());
        
        p1EnergyBar.SetMaxValue(PlayerStats.GetMaxEnergy(1));
        p1EnergyBar.SetValue(playerController.GetPlayerEnergy(1).GetEnergyAmount());
        p2EnergyBar.SetMaxValue(PlayerStats.GetMaxEnergy(2));
        p2EnergyBar.SetValue(playerController.GetPlayerEnergy(2).GetEnergyAmount());
        
        enemyHpBar.SetMaxValue(enemyController.GetMaxHealth());
        enemyHpBar.SetValue(enemyController.GetMaxHealth());
    }

    void OnEnable()
    {
        SubscribeToAllControllerEvents();
    }

    void OnDisable()
    {
        UnsubscribeFromAllControllerEvents();
    }

    private void SubscribeToAllControllerEvents()
    {
        // from player manager
        playerController.OnHealthChange += UpdatePlayerHealthBar;
        playerController.OnP1EnergyChange += UpdateP1EnergyBar;
        playerController.OnP2EnergyChange += UpdateP2EnergyBar;
        playerController.OnP1SpecialChange += UpdateP1SpecialBar;
        playerController.OnP2SpecialChange += UpdateP2SpecialBar;

        // from enemy manager
        enemyController.OnEnemyMainDamaged += UpdateEnemyHpBar;
    }

    private void UnsubscribeFromAllControllerEvents()
    {
        // from player manager
        playerController.OnHealthChange -= UpdatePlayerHealthBar;
        playerController.OnP1EnergyChange -= UpdateP1EnergyBar;
        playerController.OnP2EnergyChange -= UpdateP2EnergyBar;
        playerController.OnP1SpecialChange -= UpdateP1SpecialBar;
        playerController.OnP2SpecialChange -= UpdateP2SpecialBar;

        // from enemy manager
        enemyController.OnEnemyMainDamaged += UpdateEnemyHpBar;
    }
    private void UpdatePlayerHealthBar(float change)
    {
        playerHpBar.ChangeValueBy(change);
    }

    private void UpdateP1EnergyBar(float change)
    {
        p1EnergyBar.ChangeValueBy(change);
    }

    private void UpdateP2EnergyBar(float change)
    {
        p2EnergyBar.ChangeValueBy(change);
    }

    private void UpdateP1SpecialBar(float change)
    {
        p1SpecialBar.ChangeValueBy(change);
    }

    private void UpdateP2SpecialBar(float change)
    {
        p2SpecialBar.ChangeValueBy(change);
    }

    private void UpdateEnemyHpBar(float change)
    {
        enemyHpBar.ChangeValueBy(change);
    }
}