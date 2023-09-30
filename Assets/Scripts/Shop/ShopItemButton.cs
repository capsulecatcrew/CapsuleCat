using Player.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopItemButton : MonoBehaviour
{
    [SerializeField] [Range(1, 2)] private int playerNum = 1;

    public ButtonSprite buttonSpriteManager;

    public delegate void ButtonPressed(int playerNum);
    public event ButtonPressed OnButtonPressed;
    
    public delegate void ButtonDisable(ShopItemButton shopItemButton);
    public event ButtonDisable OnButtonDisable;
    
    [Header("UI Refs")]
    [FormerlySerializedAs("Name")] [SerializeField] private TMP_Text nameText;
    [FormerlySerializedAs("Cost")] [SerializeField] private TMP_Text costText;
    
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource; // TODO: replace with globalAudio, currently on Main Camera
    [SerializeField] private AudioClip bought; // move to global audio
    [SerializeField] private AudioClip broke;
    [SerializeField] private AudioClip disabled; // TODO: remove when UI button sound interface is made
    // TODO: disable 'pressed' sound when UI button sound interface is made
    // TODO: highlighted sound played by hitbox trigger 2D at the moment, remove on complete
    
    [FormerlySerializedAs("_usable")] [SerializeField] private bool usable;
    private int _cost;

    public void Init(string name, string stringCost, bool usable, int cost)
    {
        nameText.text = name;
        costText.text = stringCost;
        this.usable = usable;
        _cost = cost;
    }

    /// <summary>
    /// Attempts to purchase item for specified player.
    /// </summary>
    /// <param name="purchaserNum">Number of player attempting to purchase item.</param>
    public void AttemptPurchase(int purchaserNum)
    {
        if (!usable)
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
        OnButtonPressed?.Invoke(playerNum);
        GlobalAudio.Singleton.PlaySound("UI_SHOP_BOUGHT");
        Disable();
    }

    public void Disable()
    {
        nameText.text = "PURCHASED";
        costText.text = "";
        usable = false;
        
        buttonSpriteManager.SetToSpriteState(2);
        buttonSpriteManager.useable = false;
    }

    public void OnDisable()
    {
        OnButtonDisable?.Invoke(this);
    }
    
    /// <summary>
    /// Called by Hitbox Trigger 2D Trigger Enter Event
    /// </summary>
    public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");
}
