using System;
using System.Collections.Generic;
using Player.Stats.Persistent;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ShopManager : MonoBehaviour
{
    private static int _currentShopLevel = 1;

    // Saves which shop buttons were available, and if they are useable
    public static List<Tuple<UpgradeableLinearStat, bool>> ShopState = new ();

    public TMP_Text moneyCount1, moneyCount2;

    public List<ShopItemButton> p1ItemButtons;
    public List<ShopItemButton> p2ItemButtons;

    [Header("Debug")]
    public bool debug;
    public TMP_Text debugText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyCounter();
        if (PlayerStats.GetCurrentStage() != _currentShopLevel || ShopState.Count == 0)
        {
            // Only run the shop buttons randomization code once per level
            _currentShopLevel = PlayerStats.GetCurrentStage();
            RandomizeShopButtons();
            UpdateShopState();
        }
        else
        {
            // Fill in the shop with the saved shop status
            FillButtonsFromState();
        }
    }

    /// <summary>
    /// Gives each shop button a randomly linked stat.
    /// </summary>
    public void RandomizeShopButtons()
    {
        /******************************************************************************
         * Specifications:
         * - Each player can get any one of their player stats as an option.
         * - Both players can receive equivalent stat options (e.g. both can have attack power available)
         * - Only one player can receive a shared stat (e.g. HP) at a time.
         ******************************************************************************/

        // Random random = new Random();
        //
        // SetPlayerButtons(1, p1ItemButtons, random);
        // SetPlayerButtons(2, p2ItemButtons, random);
        //
        // int randButton = random.Next(0, p1ItemButtons.Count + p2ItemButtons.Count + 1);
        // if (randButton < p1ItemButtons.Count)
        // {
        //     p1ItemButtons[randButton].UpdateShopItem(PlayerStats.Health);
        // }
        // else if (randButton < p1ItemButtons.Count + p2ItemButtons.Count)
        // {
        //     p2ItemButtons[randButton - p1ItemButtons.Count].UpdateShopItem(PlayerStats.Health);
        // }
    }

    /// <summary>
    /// Randomly sets the buttons for one player with their own player stats.
    /// </summary>
    /// <param name="playerNo">1 or 2</param>
    /// <param name="buttons">list of buttons</param>
    /// <param name="rng">Random instance</param>
    private void SetPlayerButtons(int playerNo, List<ShopItemButton> buttons, Random rng)
    {
        // List<int> selectedNumbers = new List<int>();
        //
        // while (selectedNumbers.Count < buttons.Count)
        // {
        //     int randomNumber = rng.Next(0, PlayerStats.GetPlayer(playerNo).stats.Count);
        //     
        //     // Check if the randomly generated number is not already selected
        //     if (!selectedNumbers.Contains(randomNumber))
        //     {
        //         selectedNumbers.Add(randomNumber);
        //     }
        // }
        //
        // // Set shop buttons for player
        // for (int i = 0; i < buttons.Count; i++)
        // {
        //     buttons[i].UpdateShopItem(PlayerStats.GetPlayer(playerNo).stats[selectedNumbers[i]]);
        // }
    }

    public void UpdateShopState()
    {
        // // clear current state
        // ShopState.Clear();
        //
        // // add p1 item buttons
        // for (int i = 0; i < p1ItemButtons.Count; i++)
        // {
        //     ShopState.Add(new Tuple<Stat, bool>(p1ItemButtons[i].GetLinkedStat(), p1ItemButtons[i].useable));
        // }
        //
        // // add p2 item buttons
        // for (int i = 0; i < p2ItemButtons.Count; i++)
        // {
        //     ShopState.Add(new Tuple<Stat, bool>(p2ItemButtons[i].GetLinkedStat(), p2ItemButtons[i].useable));
        // }
    }

    /// <summary>
    /// Fills in the buttons from saved shop state. No effect if no saved shop state.
    /// </summary>
    void FillButtonsFromState()
    {
        // if (ShopState.Count == 0) return;
        //
        // for (int i = 0; i < ShopState.Count; i++)
        // {
        //     if (i < p1ItemButtons.Count)
        //     {
        //         p1ItemButtons[i].UpdateShopItem(ShopState[i].Item1);
        //         if (!ShopState[i].Item2)
        //         {
        //             p1ItemButtons[i].DisableButton();
        //         }
        //     }
        //     else
        //     {
        //         int j = i - p1ItemButtons.Count;
        //         p2ItemButtons[j].UpdateShopItem(ShopState[i].Item1);
        //         if (!ShopState[i].Item2)
        //         {
        //             p2ItemButtons[j].DisableButton();
        //         }
        //     }
        // }
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
