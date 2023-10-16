using Player.Stats.Templates;

namespace Shop
{
    public class StatInfoStatsPageItem: IShopItem
    {
        private readonly UpgradeableStat _linkedStat;

        public StatInfoStatsPageItem(UpgradeableStat linkedStat)
        {
            _linkedStat = linkedStat;
        }

        public void Purchase(){}
        public string GetTitle() => _linkedStat.Name + ": Lvl " + _linkedStat.Level;
        public string GetDescription() => _linkedStat.Description;
        public int GetCost() => 0;
    }
}