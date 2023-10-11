using System;
using System.Collections.Generic;
using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using Player.Stats;
using Player.Stats.Templates;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private const int HpChance = 35;
    private const int SpecialChance = 20;

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
        InitShopSpecialButtons();
        InitButtonsUsability();
    }

    private void OnDestroy()
    {
        SaveButtonsUsability();
    }

    /// <summary>
    /// Randomise stats to be upgraded by the player.
    /// <p>Each player can get any one of their player stats as an option.</p>
    /// <p>30% chance for a shared stat to appear on 1 of the players.</p>
    /// </summary>
    private static void RandomiseStats()
    {
        _chosenStats1 = PlayerStats.GetShopStats(1);
        _chosenStats2 = PlayerStats.GetShopStats(2);
        var includeHealth = Random.Range(1, 101) < HpChance;
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

    private void InitShopSpecialButtons()
    {
        var p1Special = Random.Range(1, 101) < SpecialChance;
        if (p1Special)
        {
            var chosen1 = PlayerStats.GetShopSpecialMove(1);
            switch (chosen1)
            {
                case SpecialMoveEnum.MoveHeal:
                    Heal.InitShopItemButton(player1Buttons[0]);
                    break;
                case SpecialMoveEnum.MoveAbsorbShield:
                    AbsorbShield.InitShopItemButton(player1Buttons[0]);
                    break;
                case SpecialMoveEnum.ShootVampire:
                    Vampire.InitShopItemButton(player1Buttons[0]);
                    break;
                case SpecialMoveEnum.ShootLaser:
                    Debug.Log("LASER: NOT IMPLEMENTED YET");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        var p2Special = Random.Range(1, 101) < SpecialChance;
        if (p2Special)
        {
            var chosen2 = PlayerStats.GetShopSpecialMove(2);
            switch (chosen2)
            {
                case SpecialMoveEnum.MoveHeal:
                    Heal.InitShopItemButton(player2Buttons[0]);
                    break;
                case SpecialMoveEnum.MoveAbsorbShield:
                    AbsorbShield.InitShopItemButton(player2Buttons[0]);
                    break;
                case SpecialMoveEnum.ShootVampire:
                    Vampire.InitShopItemButton(player2Buttons[0]);
                    break;
                case SpecialMoveEnum.ShootLaser:
                    Debug.Log("LASER: NOT IMPLEMENTED YET");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            if (!Buttons1Usable[i]) player1Buttons[i].DisableButton();
        }
        for (var i = 0; i < player2Buttons.Count; i++)
        {
            if (!Buttons2Usable[i]) player2Buttons[i].DisableButton();
        }
    }
    
    private void SaveButtonsUsability()
    {
        for (var i = 0; i < player1Buttons.Count; i++)
        {
            if (!player1Buttons[i].IsUsable()) Buttons1Usable[i] = false;
        }
        for (var i = 0; i < player2Buttons.Count; i++)
        {
            if (!player2Buttons[i].IsUsable()) Buttons2Usable[i] = false;
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
