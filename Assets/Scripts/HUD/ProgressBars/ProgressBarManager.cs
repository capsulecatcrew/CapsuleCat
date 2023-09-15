using HUD.ProgressBars;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Damageable playerDamageable;
    [SerializeField] private HealthBar playerHealthBar;
    
    [Header("Player Energy")]
    [SerializeField] private EnergyBar player1EnergyBar;
    [SerializeField] private EnergyBar player2EnergyBar;

    [Header("Player Special")]
    [SerializeField] private SpecialBar player1SpecialBar;
    [SerializeField] private SpecialBar player2SpecialBar;

    
}
