using System.Collections.Generic;
using Player.Stats.Templates;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private static int _currentShopLevel = 1;

    public TMP_Text moneyCount1, moneyCount2;

    [SerializeField] private List<ShopItemButton> player1Buttons;
    [SerializeField] private List<ShopItemButton> player2Buttons;
    
    private static List<UpgradeableStat> _chosenStats1;
    private static List<UpgradeableStat> _chosenStats2;

    // Start is called before the first frame update
    public void Start()
    {
        if (PlayerStats.GetCurrentStage() != _currentShopLevel)
        {
            _currentShopLevel = PlayerStats.GetCurrentStage();
            RandomiseStats();
        }
        UpdateMoneyCounter();
        InitShopButtons();
    }
    
    /// <summary>
    /// Randomise stats to be upgraded by the player.
    /// <p>Each player can get any one of their player stats as an option.</p>
    /// <p>30% chance for a shared stat to appear on 1 of the players.</p>
    /// </summary>
    private static void RandomiseStats()
    {
        _chosenStats1 = PlayerStats.GetStatsToUpgrade(1);
        _chosenStats2 = PlayerStats.GetStatsToUpgrade(2);
        var includeHealth = Random.Range(1, 10) < 4;
        if (!includeHealth) return;
        var slot = Random.Range(1, 6);
        if (slot < 4)
        {
            _chosenStats1[slot] = PlayerStats.MaxHealth;
        }
        else
        {
            slot -= 3;
            _chosenStats2[slot] = PlayerStats.MaxHealth;
        }
    }

    private void InitShopButtons()
    {
        for (var i = 0; i < 3; i++)
        {
            _chosenStats1[i].InitShopItemButton(player1Buttons[i]);
            _chosenStats2[i].InitShopItemButton(player2Buttons[i]);
        }
    }

    /// <summary>
    /// Update the money counter UI elements.
    /// </summary>
    public void UpdateMoneyCounter()
    {
        moneyCount1.text = PlayerStats.GetMoneyString(1);
        moneyCount2.text = PlayerStats.GetMoneyString(2);
    }
}
