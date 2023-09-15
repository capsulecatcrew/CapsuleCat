using TMPro;
using UnityEngine;

public class ShopItemButton
{
    [SerializeField] [Range(1, 2)] private int _playerNum = 1;

    public ButtonSprite ButtonSpriteManager;

    [Header("Text Components")]
    public TMP_Text Name;
    public TMP_Text Description;
    public TMP_Text Cost;

    [Space]
    private bool _usable;
    private Stat _stat;

    public ShopItemButton(Stat stat, string name, string description, string cost, bool usable)
    {
        _stat = stat;
        Name.text = name;
        Description.text = description;
        Cost.text = cost;
        _usable = usable;
    }

    /// <summary>
    /// Attempts to purchase item
    /// </summary>
    /// <param name="playerNo">Player attempting to purchase</param>
    public void AttemptPurchase(int playerNum)
    {
        if (!_usable) return;
        if (playerNum != _playerNum) return;
        
        if (!PlayerStats.RemovePlayerMoney(playerNum, _stat.GetCost())) return;
        
        _stat.Upgrade();
        
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
