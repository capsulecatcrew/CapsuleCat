public abstract class SpecialBurstMove : SpecialMove
{
    public SpecialBurstMove(int playerNum, float cost) : base(playerNum, cost) { }

    public override bool Start()
    {
        if (!BattleManager.HasSpecial(PlayerNum, Cost)) return false;
        BattleManager.UseSpecial(PlayerNum, Cost);
        return true;
    }

    public override void Stop() { }
}