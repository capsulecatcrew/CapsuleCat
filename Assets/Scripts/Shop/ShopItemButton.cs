using System;
using TMPro;
using UnityEngine;

public class ShopItemButton: MonoBehaviour
{
    [Range(1, 2)] public int forPlayer = 1;

    [Header("Sprite Properties")]
    public SpriteRenderer spriteRenderer;
    public Sprite normalIcon;
    public Sprite highlightedIcon;
    public Sprite disabledIcon;

    [Header("Text Components")]
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemCost;

    [Space]
    public bool useable = true;
    private PlayerStat _linkedStat;

    void Update()
    {

    }

    /// <summary>
    /// Sets the stat upgrade item the shop button is linked to.
    /// </summary>
    public void UpdateShopItem(PlayerStat playerStat)
    {
        _linkedStat = playerStat;
        if (_linkedStat != null)
        {
            itemName.text = _linkedStat.name + " " + _linkedStat.GetCurrentLevel();
            if (_linkedStat.IsMaxLevel())
            {
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
        useable = false;
    }
}
