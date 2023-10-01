using Player.Stats;

namespace Player.Special.Move
{
    public class AbsorbShield : SpecialMove
    {
        private const string Name = "Energy Shield";
        private const float Amount = 1f;
        private const string Description = "";
        
        private bool _isEnabled;
        private float _timer;
        
        public AbsorbShield(int playerNum) : base(playerNum, 3) { }

        public override void Enable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1ShieldHit += ApplyEffect;
                    return;
                case 2:
                    PlayerController.OnP2ShieldHit += ApplyEffect;
                    return;
            }
        }

        public override void Disable()
        {
            switch (PlayerNum)
            {
                case 1:
                    PlayerController.OnP1ShieldHit -= ApplyEffect;
                    return;
                case 2:
                    PlayerController.OnP2ShieldHit -= ApplyEffect;
                    return;
            }
        }

        public override void Start()
        {
            if (_isEnabled)
            {
                Stop();
                return;
            }
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            _timer = 0;
            PlayerController.EnableShield(PlayerNum);
            PlayerController.OnDeltaTimeUpdate += UpdateTimer;
            _isEnabled = true;
        }

        public override void Stop()
        {
            PlayerController.DisableShield(PlayerNum);
            PlayerController.OnDeltaTimeUpdate -= UpdateTimer;
            _isEnabled = false;
        }

        protected override void ApplyEffect(float amount)
        {
            if (!_isEnabled) return;
            if (amount >= 0) return;
            var absorbed = -amount;
            PlayerController.Heal(absorbed);
            PlayerController.AddEnergy(PlayerNum, absorbed * Amount);
        }

        private void UpdateTimer(float deltaTime)
        {
            if (!_isEnabled) return;
            if (_timer > 0) _timer -= deltaTime;
            if (!PlayerController.HasSpecial(PlayerNum, Cost))
            {
                Stop();
                return;
            }
            _timer = 1;
            PlayerController.UseSpecial(PlayerNum, Cost);
        }
        
        public static void InitShopItemButton(ShopItemButton shopItemButton)
        {
            shopItemButton.Init(GetShopItemName(), GetCostString(), true, ShopCost);
            shopItemButton.OnButtonDisable += HandleShopItemButtonDisable;
            shopItemButton.OnButtonPressed += HandleShopItemButtonPressed;
        }
        
        private static void HandleShopItemButtonPressed(int playerNum)
        {
            PlayerStats.SetSpecialMove(playerNum, SpecialMoveEnum.MoveAbsorbShield);
        }

        private static void HandleShopItemButtonDisable(ShopItemButton shopItemButton)
        {
            shopItemButton.OnButtonPressed -= HandleShopItemButtonPressed;
            shopItemButton.OnButtonDisable -= HandleShopItemButtonDisable;
        }
        
        private static string GetShopItemName()
        {
            return Name;
        }
    }
}