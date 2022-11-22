public class PlayerStat
{
    private float _statLevel;
    private readonly float _baseStat;
    private float _currentStatMax;
    private float _currentStatValue;
    private readonly float _statIncrementPerLevel;
    private readonly float _baseUpgradeCost;
    private float _currentUpgradeCost;
    private readonly float _costIncrementPerLevel;

    public PlayerStat(float baseStat, float statIncrementPerLevel, float baseUpgradeCost, float costIncrementPerLevel, float currentLevel = 1)
    {
        _statLevel = currentLevel;
        _baseStat = baseStat;
        _statIncrementPerLevel = statIncrementPerLevel;
        _currentStatMax = baseStat + _statLevel * statIncrementPerLevel;
        _currentStatValue = _currentStatMax;
        _baseUpgradeCost = baseUpgradeCost;
        _costIncrementPerLevel = costIncrementPerLevel;
        _currentUpgradeCost = baseUpgradeCost + _statLevel * costIncrementPerLevel;
    }
    
    public void ResetStats()
    {
        _statLevel = 1;
        _currentStatMax = _baseStat;
        _currentStatValue = _baseStat;
        _currentUpgradeCost = _baseUpgradeCost;
    }

    public void IncrementLevel()
    {
        _statLevel++;
        _currentStatMax += _statIncrementPerLevel;
        _currentStatValue += _statIncrementPerLevel;
        _currentUpgradeCost += _costIncrementPerLevel;
    }

    public void SetCurrentValue(float value)
    {
        _currentStatValue = value > _currentStatMax ? _currentStatMax : value;
    }

    public void IncreaseCurrentValue(float amount)
    {
        float increasedAmt = _currentStatValue + amount;
        _currentStatValue = increasedAmt > _currentStatMax ? _currentStatMax : increasedAmt;
    }

    public float GetCurrentValue()
    {
        return _currentStatValue;
    }

    public float GetMaxValue()
    {
        return _currentStatMax;
    }

    public float GetCurrentCost()
    {
        return _currentUpgradeCost;
    }
}
