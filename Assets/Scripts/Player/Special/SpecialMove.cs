using UnityEngine;

public abstract class SpecialMove
{
    private const int ShopCost = 100;
    private string _name;
    
    protected int PlayerNum;
    protected float Cost;
    protected BattleManager BattleManager;

    public SpecialMove(string name, int playerNum, float cost)
    {
        _name = name;
        PlayerNum = playerNum;
        Cost = cost;
        BattleManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
    }

    public abstract void Start();

    public abstract void Stop();

    protected abstract void ApplyEffect(float amount);

    public void InitShopItemButton(ShopItemButton shopItemButton)
    {
        // shopItemButton.Init();
    }

    private string GetCostString()
    {
        return "$" + ShopCost;
    }

    public void UpdateBattleManager(BattleManager battleManager)
    {
        BattleManager = battleManager;
    }
}