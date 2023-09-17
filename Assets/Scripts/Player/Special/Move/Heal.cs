namespace Player.Special.Move
{
    public class Heal : SpecialMove
    {
        private const int Amount = 10;

        public Heal(int playerNum) : base(playerNum, 20) { }
        
        public override void Start()
        {
            if (!BattleManager.HasSpecial(PlayerNum, Cost)) return;
            BattleManager.UseSpecial(PlayerNum, Cost);
            ApplyEffect(Amount);
        }

        public override void Stop() { }

        protected override void ApplyEffect(float amount)
        {
            BattleManager.HealPlayer(amount);
        }
    }
}