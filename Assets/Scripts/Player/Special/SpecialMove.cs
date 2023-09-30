using Battle.Controllers.Player;

namespace Player.Special
{
    public abstract class SpecialMove
    {
        protected const int ShopCost = 150;

        protected readonly int PlayerNum;
        protected readonly float Cost;

        protected PlayerController PlayerController;

        protected SpecialMove(int playerNum, float cost)
        {
            PlayerNum = playerNum;
            Cost = cost;
        }

        public abstract void Enable();

        public abstract void Disable();

        public abstract void Start();

        public abstract void Stop();

        protected abstract void ApplyEffect(float amount);

        protected static string GetCostString()
        {
            return "$" + ShopCost;
        }

        public void UpdatePlayerController(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }
}