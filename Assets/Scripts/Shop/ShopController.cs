using System.Collections.Generic;
using Player.Stats;
using Player.Stats.Templates;
using TMPro;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private static int _currentShopLevel = 1;

    public TMP_Text moneyCount1, moneyCount2;

    [SerializeField] private List<ShopItemButton> player1Buttons;
    private static readonly List<bool> Buttons1Usable = new ();
    [SerializeField] private List<ShopItemButton> player2Buttons;
    private static readonly List<bool> Buttons2Usable = new ();

    private static List<UpgradeableStat> _chosenStats1;
    private static List<UpgradeableStat> _chosenStats2;

    // Start is called before the first frame update
    public void Start()
    {
        if (PlayerStats.GetCurrentStage() != _currentShopLevel)
        {
            _currentShopLevel = PlayerStats.GetCurrentStage();
            RandomiseStats();
            ResetButtonsUsability();
        }
        UpdateMoneyCounter();
        InitShopButtons();
        InitButtonsUsability();
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
        var slot = Random.Range(0, 6);
        if (slot < 3)
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
    /// Sets saved state of all buttons to usable.
    /// </summary>
    private void ResetButtonsUsability()
    {
        Buttons1Usable.Clear();
        Buttons2Usable.Clear();
        for (var i = 0; i < player1Buttons.Count; i++)
        {
            Buttons1Usable.Add(true);
        }
        for (var i = 0; i < player2Buttons.Count; i++)
        {
            Buttons2Usable.Add(true);
        }
    }

    private void InitButtonsUsability()
    {
        for (var i = 0; i < player1Buttons.Count; i++)
        {
            if (!Buttons1Usable[i]) player1Buttons[i].Disable();
        }
        for (var i = 0; i < player2Buttons.Count; i++)
        {
            if (!Buttons2Usable[i]) player2Buttons[i].Disable();
        }
    }

    public void SavePlayer1ButtonAsUnusable(int index)
    {
        Buttons1Usable[index] = false;
    }

    public void SavePlayer2ButtonAsUnusable(int index)
    {
        Buttons2Usable[index] = false;
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
