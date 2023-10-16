using Player.Special;
using Player.Stats;

namespace Shop
{
    public class SpecialMoveShopItem: IShopItem
    {
        private readonly int _playerNum;
        private readonly SpecialMoveEnum _linkedSpecialMove;
        
        public SpecialMoveShopItem(int playerNum, SpecialMoveEnum specialMove)
        {
            _playerNum = playerNum;
            _linkedSpecialMove = specialMove;
        }
        
        public void Purchase() => PlayerStats.SetSpecialMove(_playerNum, _linkedSpecialMove);

        // TODO make special moves have name interface
        public string GetTitle()
        {
            return _linkedSpecialMove switch
            {
                SpecialMoveEnum.MoveHeal => "Special Move: Flash Heal",
                SpecialMoveEnum.MoveAbsorbShield => "Special Move: Absorb Shield",
                SpecialMoveEnum.ShootVampire => "Special Move: Vampiric Bullets",
                SpecialMoveEnum.ShootLaser => "Special Move: Laser Blast",
                _ => "ERROR"
            };
        }

        // TODO make special moves have description interface
        public string GetDescription()
        {
            return _linkedSpecialMove switch
            {
                SpecialMoveEnum.MoveHeal => "Instantly heals robot.\nSpecial Energy Cost: 10 per heal",
                SpecialMoveEnum.MoveAbsorbShield => "Creates a shield that absorbs attacks into energy.\nSpecial Energy Cost: 3 per second",
                SpecialMoveEnum.ShootVampire => "Causes bullets to heal robot upon hitting enemy.\nSpecial Energy Cost: 5 per bullet",
                SpecialMoveEnum.ShootLaser => "Fires a massive laser.\nSpecial Energy Cost: 3",
                _ => "ERROR"
            };
        }
        
        // TODO make special moves have description interface
        public int GetCost() => 150;
    }
}