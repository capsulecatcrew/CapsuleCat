public abstract class SpecialOnHitMove : SpecialMove
{
    protected bool HasApplied;
    
    public SpecialOnHitMove(int playerNum, float cost) : base(playerNum, cost)
    {
        BattleManager.OnEnemyHit += ApplyEffect;
    }

    public override bool Start()
    {
        if (!BattleManager.HasSpecial(PlayerNum, Cost)) return false;
        BattleManager.UseSpecial(PlayerNum, Cost);
        return true;
    }

    protected override void ApplyEffect(float amount)
    {
        if (HasApplied) return;
        HasApplied = true;
    }
}