using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;

namespace Shop
{
    public class SpecialMoveStatsPageItem: IShopItem
    {
        private readonly SpecialMove _linkedSpecialMove;

        public SpecialMoveStatsPageItem(SpecialMove linkedSpecialMove)
        {
            _linkedSpecialMove = linkedSpecialMove;
        }

        public void Purchase(){}

        // TODO make special moves have name interface
        public string GetTitle()
        {
            return _linkedSpecialMove switch
            {
                Heal => "Special Move: Flash Heal",
                AbsorbShield => "Special Move: Absorb Shield",
                Vampire => "Special Move: Vampiric Bullets",
                Laser => "Special Move: Laser Blast",
                _ => "No Special Move Equipped"
            };
        }

        // TODO make special moves have description interface
        public string GetDescription()
        {
            return _linkedSpecialMove switch
            {
                Heal => "Instantly heals robot.\nSpecial Energy Cost: 10 per heal",
                AbsorbShield => "Creates a shield that absorbs attacks into energy.\nSpecial Energy Cost: 3 per second",
                Vampire => "Causes bullets to heal robot upon hitting enemy.\nSpecial Energy Cost: 5 per bullet",
                Laser => "Fires a massive laser.\nSpecial Energy Cost: 3",
                _ => ""
            };
        }
        
        public int GetCost() => 0;

    }
}