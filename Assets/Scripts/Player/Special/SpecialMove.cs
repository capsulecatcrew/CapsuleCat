using Battle.Controllers.Player;

namespace Player.Special
{
    public abstract class SpecialMove
    {
        private const int ShopCost = 100;
        private string _name;

        protected readonly int PlayerNum;
        protected readonly float Cost;

        protected PlayerController PlayerController;

        protected SpecialMove(string name, int playerNum, float cost)
        {
            _name = name;
            PlayerNum = playerNum;
            Cost = cost;
        }

        public abstract void Start();

        public abstract void Stop();

        protected abstract void ApplyEffect(float amount);

        public void InitShopItemButton(ShopItemButton shopItemButton)
        {
            // shopItemButton.Init();
        }

        private string GetCostString()
        {
            return "$" + ShopCost;
        }

        public void UpdatePlayerController(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }
}