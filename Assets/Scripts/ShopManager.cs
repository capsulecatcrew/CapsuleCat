using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TMP_Text moneyCount;

    public PlayerStatScriptableObject playerHpStatScriptableObject;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyCounter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem(string itemName)
    {
        PlayerStat statToUpgrade = null;
        switch (itemName)
        {
            case "Max HP":
                statToUpgrade = PlayerStats.Hp;
                break;
            case "Firing Rate":
                statToUpgrade = PlayerStats.FiringRate;
                break;
            default:
                Debug.LogWarning("itemName" + itemName + "does not exist!");
                break;
        }
        if (statToUpgrade != null && PlayerStats.Money > statToUpgrade.GetCurrentCost())
        {
            statToUpgrade.IncrementLevel();
            UpdateMoneyCounter();
            UpdateShopItem(0);
        }

    }

    private void UpdateMoneyCounter()
    {
        moneyCount.text = "$" + PlayerStats.Money.ToString();
    }

    private void UpdateShopItem(int index)
    {
        
    }

    public void IncreaseHp()
    {
        playerHpStatScriptableObject.baseStat += 5;
    }
}
