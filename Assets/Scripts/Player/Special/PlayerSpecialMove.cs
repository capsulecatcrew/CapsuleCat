public class PlayerSpecialMove
{
    protected int playerNum;
    protected IPlayerSpecialUser User;

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
}