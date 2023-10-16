using Player.Stats.Templates;
using Unity.VisualScripting;

namespace Shop
{
    public class StatUpgradeShopItem: IShopItem
    {
        private readonly UpgradeableStat _linkedStat;

        public StatUpgradeShopItem(UpgradeableStat linkedStat)
        {
            _linkedStat = linkedStat;
        }
        
        public void Purchase() => _linkedStat.UpgradeLevel();

        public string GetTitle() => "Upgrade " + _linkedStat.Name + "\n(current lvl: " + _linkedStat.Level + ")";

        public string GetDescription() => _linkedStat.Description;

        public int GetCost() => _linkedStat.Cost;
    }
}