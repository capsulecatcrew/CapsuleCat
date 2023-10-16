using System.Threading.Tasks;
using HUD;
using Player.Stats;

namespace Player.Special.Move
{
    public class Heal : SpecialMove
    {
        private const string Name = "Flash Heal";
        private const float Amount = 2;
        private const string Description = "";

        public Heal(int playerNum) : base(playerNum, 10) { } // cost: 10

        public override void Enable() { }

        public override void Disable() { }

        public override void Start()
        {
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            PlayerController.UseSpecial(PlayerNum, Cost);
            ApplyEffect(Amount);
            PlayerSoundController.PlaySpecialEnabledSound();
            UpdateIcon();
        }

        public override void Stop(bool silent = false) { }

        protected override void ApplyEffect(float amount)
        {
            PlayerController.Heal(amount);
        }
        
        protected override async void UpdateIcon()
        {
            SpecialIcon.StartSpecial(this);
            await Task.Delay(150);
            DelayUpdate(SpecialIcon);
        }

        private void DelayUpdate(SpecialIcon specialIcon)
        {
            specialIcon.StopSpecial(this);
        }
    }
}