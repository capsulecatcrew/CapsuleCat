using System;
using TMPro;
using UnityEngine;

public class ShopItemButton: MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemDescription;
    public TMP_Text itemCost;

    public enum Stat
    {
        MaxHp,
        FiringRate,
        EnergyCapacity,
        EnergyAbsorption,
        HealthRecovery
    }

    public Stat linkedStat;
    
    public PlayerStat LinkedStat;

    private void Start()
    {
        switch (linkedStat)
        {
            case Stat.MaxHp:
                LinkedStat = PlayerStats.Hp;
                break;
            case Stat.FiringRate:
                LinkedStat = PlayerStats.FiringRate;
                break;
            case Stat.EnergyCapacity:
                LinkedStat = PlayerStats.Energy;
                break;
            case Stat.EnergyAbsorption:
                LinkedStat = PlayerStats.EnergyAbsorb;
                break;
            default:
                break;
        }
        UpdateItemText();
    }

    public void UpdateItemText()
    {
        if (LinkedStat != null)
        {
            itemDescription.text = "Lv. " + LinkedStat.GetCurrentLevel();
            if (LinkedStat.IsMaxLevel())
            {
                itemCost.text = "Max Level";
            } 
            else
            {
                itemCost.text = "Upgrade - $" + LinkedStat.GetCurrentCost();
            }
        }
        else
        {
            Debug.LogWarning("No Player Stat linked to button!");
        }
    }
}
