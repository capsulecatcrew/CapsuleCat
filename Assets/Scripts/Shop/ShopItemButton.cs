using Player.Stats.Templates;
using TMPro;
using UnityEngine;

public class ShopItemButton
{
    [SerializeField] [Range(1, 2)] private int _playerNum = 1;

    public ButtonSprite ButtonSpriteManager;

    private UpgradeableStat _stat;

    [Header("Text Components")]
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Cost;

    [Space]
    private bool _usable;
    private int _cost;

    public ShopItemButton(UpgradeableStat stat, string name, string description, string stringCost, bool usable, int cost)
    {
        _stat = stat;
        Name.text = name;
        Description.text = description;
        Cost.text = stringCost;
        _usable = usable;
        _cost = cost;
    }

    /// <summary>
    /// Attempts to purchase item for specified player.
    /// </summary>
    /// <param name="playerNum">Player attempting to purchase item.</param>
    public void AttemptPurchase(int playerNum)
    {
        if (!_usable) return;
        if (playerNum != _playerNum) return;
        
        if (!PlayerStats.RemoveMoney(playerNum, _cost)) return;
        _stat.UpgradeLevel();

        Disable();
    }

    private void Disable()
    {
        Name.text = "PURCHASED";
        _usable = false;
        
        ButtonSpriteManager.SetToSpriteState(2);
        ButtonSpriteManager.useable = false;
    }

    public void SetColor(Color color)
    {
        ButtonSpriteManager.SetColor(color);
    }
}
