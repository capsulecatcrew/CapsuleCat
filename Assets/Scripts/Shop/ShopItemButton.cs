using System;
using TMPro;
using UnityEngine;

public class ShopItemButton: MonoBehaviour
{
    [Range(1, 2)] public int forPlayer = 1;

    public ButtonSprite buttonSpriteManager;

    [Header("Text Components")]
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemCost;

    [Space]
    public bool useable = true;
    private PlayerStat _linkedStat;

    /// <summary>
    /// Sets the stat upgrade item the shop button is linked to.
    /// </summary>
    public void UpdateShopItem(PlayerStat playerStat)
    {
        _linkedStat = playerStat;
        if (_linkedStat != null)
        {
            itemName.text = _linkedStat.name + " " + (_linkedStat.GetCurrentLevel() + 1);
            if (_linkedStat.IsMaxLevel())
            {
                itemName.text = _linkedStat.name;
                itemCost.text = "Max Level";
                useable = false;
            }
            else 
            {
                itemCost.text = "$" + _linkedStat.GetCurrentCost();
            }
        }
        else
        {
            Debug.LogWarning("No Player Stat linked to button!");
        }

    }

    /// <summary>
    /// Attempts to purchase item
    /// </summary>
    /// <param name="playerNo">Player attempting to purchase</param>
    public void AttemptPurchase(int playerNo)
    {
        if (!useable) return;
        if (playerNo != forPlayer) return;

        int playerMoney = PlayerStats.GetPlayer(playerNo).Money;
        
        float cost = _linkedStat.GetCurrentCost();
        if (playerMoney < cost) return;

        // Spend money
        PlayerStats.GetPlayer(playerNo).Money -= (int) _linkedStat.GetCurrentCost();

        // Upgrade stat
        _linkedStat.IncrementLevel();

        // Update button info
        UpdateShopItem(_linkedStat);

        // Disable button
        DisableButton();
    }

    public void DisableButton()
    {
        itemName.text = "DISABLED";
        // Set Sprite to disabled version
        buttonSpriteManager.SetToSpriteState(2);
        buttonSpriteManager.useable = false;

        useable = false;
    }

    public void SetColor(Color color)
    {
        buttonSpriteManager.SetColor(color);
    }
    
    public PlayerStat GetLinkedStat()
    {
        return _linkedStat;
    }
}
