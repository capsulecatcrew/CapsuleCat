namespace Player.Special.Move
{
    public class Heal : SpecialBurstMove
    {
        private const int Amount = 10;

        public Heal(int playerNum) : base(playerNum, 20) { }
        
        public new bool Start()
        {
            if (!base.Start()) return false;
            ApplyEffect(Amount);
            return true;
        }

        protected override void ApplyEffect(float amount)
        {
            BattleManager.HealPlayer(amount);
        }
    }
}