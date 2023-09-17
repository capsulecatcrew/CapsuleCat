public abstract class SpecialContinuousMove : SpecialMove
{
    private bool _isActive;
    private const float MaxCooldown = 1;
    private float _cooldown;
    private readonly float _minStartCost;

    public SpecialContinuousMove(int playerNum, float cost, float minStartCost) : base(playerNum, cost)
    {
        BattleManager.OnTimeChanged += UpdateTime;
        _minStartCost = minStartCost;
    }

    public override bool Start()
    {
        if (_isActive) return false;
        if (!BattleManager.HasSpecial(PlayerNum, _minStartCost)) return false;
        _isActive = true;
        return true;
    }

    public override void Stop()
    {
        _isActive = false;
    }

    private void UpdateTime(float deltaTime)
    {
        if (_cooldown > 0)
        {
            _cooldown -= deltaTime;
            return;
        }

        if (!_isActive) return;
        if (!BattleManager.HasSpecial(PlayerNum, Cost))
        {
            Stop();
            return;
        }

        ApplyEffect(0);
        _cooldown = MaxCooldown;
    }
}