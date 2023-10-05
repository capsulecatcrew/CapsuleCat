using Battle.Controllers.Player;
using HUD;

namespace Player.Special
{
    public abstract class SpecialMove
    {
        protected const int ShopCost = 150;

        protected readonly int PlayerNum;
        protected readonly float Cost;

        protected PlayerController PlayerController;
        protected PlayerSoundController PlayerSoundController;
        
        protected SpecialIcon SpecialIcon;

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

        public void SetIcon(SpecialIcon specialIcon)
        {
            SpecialIcon = specialIcon;
        }

        protected abstract void UpdateIcon();

        protected static string GetCostString()
        {
            return "$" + ShopCost;
        }

        public void UpdateControllers(PlayerController playerController, PlayerSoundController playerSoundController)
        {
            PlayerController = playerController;
            PlayerSoundController = playerSoundController;
        }
    }
}