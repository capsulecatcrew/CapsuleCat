using System;
using System.Collections.Generic;
using Player.Special;
using Player.Stats;
using Player.Stats.Templates;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        private static int _currentShopLevel = 1;

        [SerializeField] private SpecialMoveSpriteScriptableObject specialMoveSprites;
        [SerializeField] private StatUpgradeSpriteScriptableObject statUpgradeSprites;
    
        public TMP_Text moneyCount1, moneyCount2;

        [SerializeField] private List<ShopItemButton> player1Buttons;
        private static readonly List<bool> Buttons1Usable = new ();
        [SerializeField] private List<ShopItemButton> player2Buttons;
        private static readonly List<bool> Buttons2Usable = new ();

        private static List<UpgradeableStat> _chosenStats1;
        private static List<UpgradeableStat> _chosenStats2;

        private static List<Sprite> _sprites1;
        private static List<Sprite> _sprites2;
    
        private const int HpChance = 35;
        private const int SpecialChance = 20;

        // Start is called before the first frame update
        public void Start()
        {
            if (PlayerStats.GetCurrentStage() != _currentShopLevel)
            {
                _currentShopLevel = PlayerStats.GetCurrentStage();
                RandomiseStats();
                ResetButtonsUsability();
            }
            UpdateMoneyCounter();
            InitShopButtons();
            InitShopSpecialButtons();
            InitButtonsUsability();
        }

        private void OnDestroy()
        {
            SaveButtonsUsability();
        }

        /// <summary>
        /// Randomise stats to be upgraded by the player.
        /// <p>Each player can get any one of their player stats as an option.</p>
        /// <p>30% chance for a shared stat to appear on 1 of the players.</p>
        /// </summary>
        private static void RandomiseStats()
        {
            _chosenStats1 = PlayerStats.GetShopStats(1);
            _chosenStats2 = PlayerStats.GetShopStats(2);
            var includeHealth = Random.Range(1, 101) < HpChance;
            if (!includeHealth) return;
            var slot = Random.Range(0, 6);
            if (slot < 3)
            {
                _chosenStats1[slot] = PlayerStats.MaxHealth;
            }
            else
            {
                slot -= 3;
                _chosenStats2[slot] = PlayerStats.MaxHealth;
            }
        }

        private void InitShopButtons()
        {
            for (var i = 0; i < 3; i++)
            {
                player1Buttons[i].Init(new StatUpgradeShopItem(_chosenStats1[i]), statUpgradeSprites.GetSprite(_chosenStats1[i]));
                player2Buttons[i].Init(new StatUpgradeShopItem(_chosenStats2[i]), statUpgradeSprites.GetSprite(_chosenStats2[i]));
            }
        }

        private void InitShopSpecialButtons()
        {
            var p1Special = Random.Range(1, 101) < SpecialChance;
            if (p1Special)
            {
                var chosen1 = PlayerStats.GetShopSpecialMove(1);
                switch (chosen1)
                {
                    case SpecialMoveEnum.MoveHeal:
                        player1Buttons[0].Init(new SpecialMoveShopItem(1, chosen1), specialMoveSprites.healSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.MoveAbsorbShield:
                        player1Buttons[0].Init(new SpecialMoveShopItem(1, chosen1), specialMoveSprites.absorbShieldSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.ShootVampire:
                        player1Buttons[0].Init(new SpecialMoveShopItem(1, chosen1), specialMoveSprites.vampireSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.ShootLaser:
                        player1Buttons[0].Init(new SpecialMoveShopItem(1, chosen1), specialMoveSprites.laserSprite.mainSprite);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            var p2Special = Random.Range(1, 101) < SpecialChance;
            if (p2Special)
            {
                var chosen2 = PlayerStats.GetShopSpecialMove(2);
                switch (chosen2)
                {
                    case SpecialMoveEnum.MoveHeal:
                        player2Buttons[0].Init(new SpecialMoveShopItem(2, chosen2), specialMoveSprites.healSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.MoveAbsorbShield:
                        player2Buttons[0].Init(new SpecialMoveShopItem(2, chosen2), specialMoveSprites.absorbShieldSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.ShootVampire:
                        player2Buttons[0].Init(new SpecialMoveShopItem(2, chosen2), specialMoveSprites.vampireSprite.mainSprite);
                        break;
                    case SpecialMoveEnum.ShootLaser:
                        player2Buttons[0].Init(new SpecialMoveShopItem(2, chosen2), specialMoveSprites.laserSprite.mainSprite);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Sets saved state of all buttons to usable.
        /// </summary>
        private void ResetButtonsUsability()
        {
            Debug.Log("resetting usability");
            Buttons1Usable.Clear();
            Buttons2Usable.Clear();
            for (var i = 0; i < player1Buttons.Count; i++)
            {
                Buttons1Usable.Add(true);
            }
            for (var i = 0; i < player2Buttons.Count; i++)
            {
                Buttons2Usable.Add(true);
            }
        }

        private void InitButtonsUsability()
        {
            for (var i = 0; i < player1Buttons.Count; i++)
            {
                if (!Buttons1Usable[i]) {player1Buttons[i].DisableButton();}
            }
            for (var i = 0; i < player2Buttons.Count; i++)
            {
                if (!Buttons2Usable[i]) player2Buttons[i].DisableButton();
            }
        }
    
        private void SaveButtonsUsability()
        {
            for (var i = 0; i < player1Buttons.Count; i++)
            {
                if (!player1Buttons[i].IsUsable()) Buttons1Usable[i] = false;
            }
            for (var i = 0; i < player2Buttons.Count; i++)
            {
                if (!player2Buttons[i].IsUsable()) Buttons2Usable[i] = false;
            }
        }
    
        /// <summary>
        /// Update the money counter UI elements.
        /// </summary>
        public void UpdateMoneyCounter()
        {
            moneyCount1.text = PlayerStats.GetMoneyString(1);
            moneyCount2.text = PlayerStats.GetMoneyString(2);
        }
    }
}
