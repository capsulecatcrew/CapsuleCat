namespace Player.Special.Shoot
{
    public class Vampire : SpecialMove
    {
        private float _minStartCost;
        
        public Vampire(int playerNum, float cost, float minStartCost) : base(playerNum, cost)
        {
            _minStartCost = minStartCost;
        }

        public override void Start()
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyEffect(float amount)
        {
            throw new System.NotImplementedException();
        }
    }
}