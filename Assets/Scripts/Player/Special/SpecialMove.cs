using UnityEngine;

public abstract class SpecialMove
{
    protected int PlayerNum;
    protected float Cost;
    protected BattleManager BattleManager;

    public SpecialMove(int playerNum, float cost)
    {
        PlayerNum = playerNum;
        Cost = cost;
        BattleManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleManager>();
    }

    public abstract bool Start();

    public abstract void Stop();

    protected abstract void ApplyEffect(float amount);
}