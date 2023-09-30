using Player.Stats;
using Player.Stats.Templates;
using TMPro;
using UnityEngine;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerNum = 1;

    public ButtonSprite buttonSpriteManager;

    private UpgradeableStat _stat;
    
    [Header("UI Refs")]
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Cost;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // TODO: replace with globalAudio, currently on Main Camera
    [SerializeField] private AudioClip bought; // move to global audio
    [SerializeField] private AudioClip broke;
    [SerializeField] private AudioClip disabled; // TODO: remove when UI button sound interface is made
    // TODO: disable 'pressed' sound when UI button sound interface is made
    // TODO: highlighted sound played by hitbox trigger 2D at the moment, remove on complete
    
    [SerializeField] private bool _usable;
    private int _cost;

    public void Init(UpgradeableStat stat, string name, string stringCost, bool usable, int cost)
    {
        _stat = stat;
        Name.text = name;
        Cost.text = stringCost;
        _usable = usable;
        _cost = cost;
    }

    /// <summary>
    /// Attempts to purchase item for specified player.
    /// </summary>
    /// <param name="purchaserNum">Number of player attempting to purchase item.</param>
    public void AttemptPurchase(int purchaserNum)
    {
        if (!_usable)
        {
            GlobalAudio.Singleton.PlaySound("UI_BTN_DISABLED");
            return;
        }
        
        if (purchaserNum != playerNum)
        {
            GlobalAudio.Singleton.PlaySound("UI_BTN_DISABLED");
            return;
        }
        if (!PlayerStats.RemoveMoney(playerNum, _cost))
        {
            GlobalAudio.Singleton.PlaySound("UI_SHOP_BROKE");
            return;
        }
        
        _stat.UpgradeLevel();
        GlobalAudio.Singleton.PlaySound("UI_SHOP_BOUGHT");
        Disable();
    }

    public void Disable()
    {
        Name.text = "PURCHASED";
        Cost.text = "";
        _usable = false;
        
        buttonSpriteManager.SetToSpriteState(2);
        buttonSpriteManager.useable = false;
    }
    
    /// <summary>
    /// Called by Hitbox Trigger 2D Trigger Enter Event
    /// </summary>
    public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");
}
