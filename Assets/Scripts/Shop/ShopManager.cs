using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class ShopManager : MonoBehaviour
{
    private static int LastLevelCompleted = 0;
    public TMP_Text moneyCount1, moneyCount2;

    public List<ShopItemButton> p1ItemButtons;
    public List<ShopItemButton> p2ItemButtons;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyCounter();
        if (PlayerStats.LevelsCompleted != LastLevelCompleted || LastLevelCompleted == 0)
        {
            // Only run the shop buttons randomization code once per level
            LastLevelCompleted = PlayerStats.LevelsCompleted;
            RandomizeShopButtons();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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

        Random random = new Random();

        // Set the player buttons with their own stat upgrades
        SetPlayerButtons(1, p1ItemButtons, random);
        SetPlayerButtons(2, p2ItemButtons, random);

        // Set a random button to a shared stat (HP only for now.)
        int randButton = random.Next(0, p1ItemButtons.Count + p2ItemButtons.Count + 1);
        if (randButton < p1ItemButtons.Count)
        {
            p1ItemButtons[randButton].UpdateShopItem(PlayerStats.Hp);
        }
        else if (randButton < p1ItemButtons.Count + p2ItemButtons.Count)
        {
            p2ItemButtons[randButton - p1ItemButtons.Count].UpdateShopItem(PlayerStats.Hp);
        }
        else
        {
            // No buttons are changed.
        }
    }

    /// <summary>
    /// Randomly sets the buttons for one player with their ownm player stats.
    /// </summary>
    /// <param name="playerNo">1 or 2</param>
    /// <param name="buttons">list of buttons</param>
    /// <param name="rng">Random instance</param>
    private void SetPlayerButtons(int playerNo, List<ShopItemButton> buttons, Random rng)
    {
        List<int> selectedNumbers = new List<int>();

        while (selectedNumbers.Count < buttons.Count)
        {
            int randomNumber = rng.Next(0, PlayerStats.GetPlayer(playerNo).stats.Count);
            
            // Check if the randomly generated number is not already selected
            if (!selectedNumbers.Contains(randomNumber))
            {
                selectedNumbers.Add(randomNumber);
            }
        }

        // Set shop buttons for player
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].UpdateShopItem(PlayerStats.GetPlayer(playerNo).stats[selectedNumbers[i]]);
            // buttons[i].SetColor(playerNo == 1 ? Color.green : Color.red);
        }
    }

    /// <summary>
    /// Update the money counter UI elements.
    /// </summary>
    public void UpdateMoneyCounter()
    {
        moneyCount1.text = "$" + PlayerStats.Player1.Money.ToString();
        moneyCount2.text = "$" + PlayerStats.Player2.Money.ToString();
    }

}
