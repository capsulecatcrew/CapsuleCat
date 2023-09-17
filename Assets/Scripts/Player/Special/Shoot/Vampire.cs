namespace Player.Special.Shoot
{
    public class Vampire : SpecialContinuousMove
    {
        public Vampire(int playerNum, float cost, float minStartCost) : base(playerNum, cost, minStartCost)
        {
        }

        protected override void ApplyEffect(float amount)
        {
            throw new System.NotImplementedException();
        }
    }
}