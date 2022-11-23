using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public TMP_Text moneyCount;

    public List<ShopItemButton> itemButtons;

    // Start is called before the first frame update
    void Start()
    {
        UpdateMoneyCounter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuyItem(int index)
    {
        PlayerStat statToUpgrade = itemButtons[index].LinkedStat;
        if (statToUpgrade == null)
        {
            Debug.Log("Stat is null");
            return;
        }

        if (PlayerStats.Money < statToUpgrade.GetCurrentCost())
        {
            return;
        }
        statToUpgrade.IncrementLevel();
        UpdateMoneyCounter();
        UpdateShopItem(index);

    }

    private void UpdateMoneyCounter()
    {
        moneyCount.text = "$" + PlayerStats.Money.ToString();
    }

    private void UpdateShopItem(int index)
    {
        itemButtons[index].UpdateItemText();
    }
}
