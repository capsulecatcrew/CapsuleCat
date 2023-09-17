using Player.Stats.Templates;
using TMPro;
using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerNum = 1;

    public ButtonSprite buttonSpriteManager;

    private UpgradeableStat _stat;
    
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Cost;
    
    private bool _usable;
    private int _cost;

    public void Init(UpgradeableStat stat, string name, string stringCost, bool usable, int cost)
    {
        _stat = stat;
        Name.text = name;
        Cost.text = stringCost;
        _usable = usable;
        _cost = cost;
        print("what");
    }

    /// <summary>
    /// Attempts to purchase item for specified player.
    /// </summary>
    /// <param name="purchaserNum">Number of player attempting to purchase item.</param>
    public void AttemptPurchase(int purchaserNum)
    {
        if (!_usable) return;
        if (purchaserNum != playerNum) return;
        if (!PlayerStats.RemoveMoney(playerNum, _cost)) return;
        
        _stat.UpgradeLevel();
        Disable();
    }

    private void Disable()
    {
        Name.text = "PURCHASED";
        _usable = false;
        
        buttonSpriteManager.SetToSpriteState(2);
        buttonSpriteManager.useable = false;
    }
}
