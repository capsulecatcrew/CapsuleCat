using Battle.Controllers.Player;
using Enemy;
using UnityEngine;

public abstract class SpecialMove
{
    private const int ShopCost = 100;
    private string _name;
    
    protected int PlayerNum;
    protected float Cost;

    protected EnemyController EnemyController;
    protected PlayerController PlayerController;

    public SpecialMove(string name, int playerNum, float cost)
    {
        _name = name;
        PlayerNum = playerNum;
        Cost = cost;
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

    public void UpdateBattleControllers(PlayerController playerController, EnemyController enemyController)
    {
        PlayerController = playerController;
        EnemyController = enemyController;
    }
}