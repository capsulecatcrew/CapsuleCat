namespace Player.Special.Move
{
    public class Heal : SpecialMove
    {
        private const string Name = "Flash Heal";
        private const int Amount = 2;

        public Heal(int playerNum) : base(Name, playerNum, 10) { }
        
        public override void Start()
        {
            if (!PlayerController.HasSpecial(PlayerNum, Cost)) return;
            PlayerController.UseSpecial(PlayerNum, Cost);
            ApplyEffect(Amount);
        }

        public override void Stop() { }

        protected override void ApplyEffect(float amount)
        {
            PlayerController.HealPlayer(amount);
        }
    }
}