using System.Text;

public class Stat
{
    private readonly string _name;
    
    protected int Level = 1;
    protected readonly int MaxLevel;
    
    protected readonly float BaseValue;
    protected readonly float ChangeValue;
    protected float Value;

    private readonly int _baseCost;
    private readonly int _addCost;
    private int _cost;
    
    public delegate void StatUpdate(int level, float value, int cost);

    public event StatUpdate OnStatUpdate;

    protected Stat(string name, int maxLevel, float baseValue, float changeValue, int baseCost, int addCost)
    {
        _name = name;
        MaxLevel = maxLevel;
        BaseValue = baseValue;
        ChangeValue = changeValue;
        _baseCost = baseCost;
        _addCost = addCost;
        
        Value = baseValue;
        _cost = _baseCost;
    }

    public void Upgrade()
    {
        Level++;
        _cost += _addCost;
        OnStatUpdate?.Invoke(Level, Value, _cost);
    }

    protected void SetLevel(int level)
    {
        Level = level;
        if (IsMaxLevel()) Level = MaxLevel;
        _cost = _baseCost + (Level - 1) * _addCost;
        OnStatUpdate?.Invoke(Level, Value, _cost);
    }
    
    public void Reset()
    {
        Level = 1;
        Value = BaseValue;
        _cost = _baseCost;
    }
    
    public bool IsWithinBounds(float value)
    {
        return value <= Value;
    }

    private string GetShopItemName()
    {
        var shopItemNameBuilder = new StringBuilder();
        shopItemNameBuilder.Append(_name);
        if (!IsMaxLevel())
        {
            shopItemNameBuilder.Append(" ");
            shopItemNameBuilder.Append(Level);
        }
        return shopItemNameBuilder.ToString();
    }

    private string GetCostString()
    {
        return IsMaxLevel() ? "MAX LEVEL" : "$" + _cost;
    }
    
    protected bool IsMaxLevel()
    {
        return Level >= MaxLevel;
    }

    public float GetValue()
    {
        return Value;
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