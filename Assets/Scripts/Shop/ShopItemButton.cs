using System;
using Player.Stats;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shop
{
    public class ShopItemButton : MonoBehaviour
    {
        private enum UsableByPlayer
        {
            Both,
            Player1,
            Player2
        }

        [SerializeField] private UsableByPlayer usableByPlayer = UsableByPlayer.Both;

        private IShopItem _shopItem;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private ShopDescriptionField p1DescriptionField;
        [SerializeField] private ShopDescriptionField p2DescriptionField;
        [SerializeField] private HitboxTrigger2D hitboxTrigger2D;
        
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
            if (usableByPlayer != UsableByPlayer.Both && purchaserNum != (int) usableByPlayer)
            {
                GlobalAudio.Singleton.PlaySound("UI_BTN_DISABLED");
                return;
            }
            if (!PlayerStats.RemoveMoney(purchaserNum, _cost))
            {
                GlobalAudio.Singleton.PlaySound("UI_SHOP_BROKE");
                return;
            }
            OnButtonPressed?.Invoke(purchaserNum);
            _shopItem.Purchase();
            GlobalAudio.Singleton.PlaySound("UI_SHOP_BOUGHT");
            ShowDescription(purchaserNum);
            DisableButton();
        }

        public void DisableButton()
        {
            usable = false;
            
            buttonSpriteManager.SetToSpriteState(2);
            buttonSpriteManager.useable = false;
        }

        private void OnEnable()
        {
            hitboxTrigger2D.HitboxEnter += ShowDescription;
        }

        private void OnDisable()
        {
            OnButtonDisable?.Invoke(this);
            hitboxTrigger2D.HitboxEnter -= ShowDescription;
        }

        public bool IsUsable()
        {
            return usable;
        }
    
        /// <summary>
        /// Called by Hitbox Trigger 2D Trigger Enter Event
        /// </summary>
        public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");
        
        private void ShowDescription(Collider2D other)
        {
            if (other.CompareTag("Player1") && usableByPlayer != UsableByPlayer.Player2)
            {
                p1DescriptionField.UpdateDescription(_shopItem, !usable);
            } 
            else if (other.CompareTag("Player2") && usableByPlayer != UsableByPlayer.Player1)
            {
                p2DescriptionField.UpdateDescription(_shopItem, !usable);
            }
        }
        
        private void ShowDescription(int playerNum)
        {
            if (playerNum == 1 && usableByPlayer != UsableByPlayer.Player2)
            {
                p1DescriptionField.UpdateDescription(_shopItem, !usable);
            } 
            else if (playerNum == 2 && usableByPlayer != UsableByPlayer.Player1)
            {
                p2DescriptionField.UpdateDescription(_shopItem, !usable);
            }
        }

    }
}
