using Player.Stats;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shop
{
    public class ShopItemButton : MonoBehaviour
    {
        [SerializeField] [Range(1, 2)] protected int playerNum = 1;

        private IShopItem _shopItem;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private ShopDescriptionField descriptionField;
        
        public ButtonSprite buttonSpriteManager;
        public delegate void ButtonPressed(int playerNum);
        public event ButtonPressed OnButtonPressed;
    
        public delegate void ButtonDisable(ShopItemButton shopItemButton);
        public event ButtonDisable OnButtonDisable;
    
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource; // TODO: replace with globalAudio, currently on Main Camera
        [SerializeField] private AudioClip bought; // move to global audio
        [SerializeField] private AudioClip broke;
        [SerializeField] private AudioClip disabled; // TODO: remove when UI button sound interface is made
        // TODO: disable 'pressed' sound when UI button sound interface is made
        // TODO: highlighted sound played by hitbox trigger 2D at the moment, remove on complete
    
        [SerializeField] private bool usable = true;
        private int _cost;

        public void Init(IShopItem shopItem, Sprite itemSprite)
        {
            _shopItem = shopItem;
            _cost = shopItem.GetCost();
            itemSpriteRenderer.sprite = itemSprite;
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
            _shopItem.Purchase();
            GlobalAudio.Singleton.PlaySound("UI_SHOP_BOUGHT");
            DisableButton();
        }

        public void DisableButton()
        {
            usable = false;
            
            buttonSpriteManager.SetToSpriteState(2);
            buttonSpriteManager.useable = false;
            ShowDescription();
        }
    
        public void OnDisable()
        {
            OnButtonDisable?.Invoke(this);
        }

        public bool IsUsable()
        {
            return usable;
        }
    
        /// <summary>
        /// Called by Hitbox Trigger 2D Trigger Enter Event
        /// </summary>
        public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");

        /// <summary>
        /// Called by Hitbox Trigger 2D Trigger Enter Event
        /// </summary>
        public void ShowDescription()
        {
            descriptionField.UpdateDescription(_shopItem, !usable);
        }

    }
}
