public class PlayerStat
{
    public readonly string name;
    private int _statLevel;
    private readonly int _maxLevel;
    private readonly float _baseStat;
    private float _currentStatMax;
    private float _currentStatValue;
    private readonly float _statIncrementPerLevel;
    private readonly int _baseUpgradeCost;
    private int _currentUpgradeCost;
    private readonly int _costIncrementPerLevel;

    public PlayerStat(string name, float baseStat, float statIncrementPerLevel, int baseUpgradeCost, int costIncrementPerLevel, int maxLevel = 10, int currentLevel = 0)
    {
        this.name = name;
        _maxLevel = maxLevel;
        _statLevel = currentLevel;
        _baseStat = baseStat;
        _statIncrementPerLevel = statIncrementPerLevel;
        _currentStatMax = baseStat + _statLevel * statIncrementPerLevel;
        _currentStatValue = _currentStatMax;
        _baseUpgradeCost = baseUpgradeCost;
        _costIncrementPerLevel = costIncrementPerLevel;
        _currentUpgradeCost = baseUpgradeCost + _statLevel * costIncrementPerLevel;
    }
    
    public void ResetStat()
    {
        _statLevel = 0;
        _currentStatMax = _baseStat;
        _currentStatValue = _baseStat;
        _currentUpgradeCost = _baseUpgradeCost;
    }

    public bool IncrementLevel()
    {
        if (_statLevel == _maxLevel) return false;
        _statLevel++;
        _currentStatMax += _statIncrementPerLevel;
        _currentStatValue += _statIncrementPerLevel;
        _currentUpgradeCost += _costIncrementPerLevel;
        return true;
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

    public float GetCurrentLevel()
    {
        return _statLevel;
    }
    
    public bool IsMaxLevel()
    {
        return _statLevel == _maxLevel;
    }

}
