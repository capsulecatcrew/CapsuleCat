public class PlayerSpecialMove
{
    protected int playerNum;
    protected IPlayerSpecialUser User;
    protected Damageable Player;
    protected Damageable Enemy;

    public void Init(IPlayerSpecialUser user)
    {
        User = user;
    }

    public bool Use()
    {
        return false;
    }
    
    public void End() { }

    public void ApplyEffect() { }

    public void SetPlayer(Damageable player)
    {
        Player = player;
    }

    public void SetEnemy(Damageable enemy)
    {
        Enemy = enemy;
    }
}