using System.Text;

public class Stat
{
    protected readonly string _name;
    
    protected int _level = 1;
    protected readonly int _maxLevel;
    
    protected readonly float _baseValue;
    protected readonly float _changeValue;
    protected float _value;
    
    protected readonly int _baseCost;
    protected readonly int _addCost;
    protected int _cost;
    
    public delegate void StatUpdate(int level, float value, int cost);

    public event StatUpdate OnStatUpdate;

    public Stat(string name, int maxLevel, float baseValue, float changeValue, int baseCost, int addCost)
    {
        _name = name;
        _maxLevel = maxLevel;
        _baseValue = baseValue;
        _changeValue = changeValue;
        _baseCost = baseCost;
        _addCost = addCost;
    }

    public void Upgrade()
    {
        _level++;
        _cost += _addCost;
        OnStatUpdate?.Invoke(_level, _value, _cost);
    }

    public void SetLevel(int level)
    {
        _level = level;
        _cost = _baseCost + (_cost - 1) * _addCost;
        OnStatUpdate?.Invoke(_level, _value, _cost);
    }
    
    public void Reset()
    {
        _level = 0;
        _value = _baseValue;
        _cost = _baseCost;
    }

    /// <summary>
    /// Check if a value is lower than the current value of this stat.
    /// </summary>
    public bool IsWithinBounds(float value)
    {
        return value <= _value;
    }

    private string GetShopItemName()
    {
        var shopItemNameBuilder = new StringBuilder();
        shopItemNameBuilder.Append(_name);
        if (!IsMaxLevel())
        {
            shopItemNameBuilder.Append(" ");
            shopItemNameBuilder.Append(_level);
        }
        return shopItemNameBuilder.ToString();
    }

    private string GetCostString()
    {
        return IsMaxLevel() ? "MAX LEVEL" : "$" + _cost;
    }
    
    protected bool IsMaxLevel()
    {
        return _level == _maxLevel;
    }

    public float GetValue()
    {
        return _value;
    }

    public ShopItemButton GetShopButton()
    {
        return new ShopItemButton(this, GetShopItemName(), "", GetCostString(), IsMaxLevel());
    }

    public int GetCost()
    {
        return _cost;
    }
}