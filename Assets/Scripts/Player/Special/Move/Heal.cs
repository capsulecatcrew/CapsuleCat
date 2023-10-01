using Player.Stats;

namespace Player.Special.Move
{
    public class Heal : SpecialMove
    {
        private const string Name = "Flash Heal";
        private const float Amount = 2;
        private const string Description = "";

        public Heal(int playerNum) : base(playerNum, 10) { }

        public override void Enable() { }

        public override void Disable() { }

        public override void Start()
        {
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            PlayerController.UseSpecial(PlayerNum, Cost);
            ApplyEffect(Amount);
        }

        public override void Stop() { }

        protected override void ApplyEffect(float amount)
        {
            PlayerController.Heal(amount);
        }
        
        public static void InitShopItemButton(ShopItemButton shopItemButton)
        {
            shopItemButton.Init(GetShopItemName(), GetCostString(), true, ShopCost);
            shopItemButton.OnButtonDisable += HandleShopItemButtonDisable;
            shopItemButton.OnButtonPressed += HandleShopItemButtonPressed;
        }
        
        private static void HandleShopItemButtonPressed(int playerNum)
        {
            PlayerStats.SetSpecialMove(playerNum, SpecialMoveEnum.ShootVampire);
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