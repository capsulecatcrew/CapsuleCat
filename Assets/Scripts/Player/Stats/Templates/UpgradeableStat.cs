using System.Text;

namespace Player.Stats.Templates
{
    public class UpgradeableStat : Stat
    {
        protected int Level = 1;
        protected readonly int MaxLevel;
        
        protected readonly float ChangeValue;

        protected int Cost;
        private readonly int _baseCost;
        private readonly int _changeCost;

        public delegate void StatUpdate(int level, float value, int cost);

        public event StatUpdate OnStatUpdate;
        
        protected UpgradeableStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost) :
            base (name, baseValue)
        {
            MaxLevel = maxLevel;
            ChangeValue = changeValue;
            Cost = baseCost;
            _baseCost = baseCost;
            _changeCost = changeCost;
        }
        
        /// <summary>
        /// Resets the stat's level to 1.
        /// <p>Resets the stat's value to base value.</p>
        /// <p>Resets the stat's cost to base cost.</p>
        /// <p>Invokes OnStatReset, OnStatUpdate events.</p>
        /// </summary>
        public new void Reset()
        {
            Level = 1;
            Cost = _baseCost;
            base.Reset();
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }
        
        /// <summary>
        /// Upgrades the stat's level, value and cost.
        /// <p> Does not upgrade if stat is at max level.</p>
        /// <p>Invokes OnStatUpdate event if not at max level.</p>
        /// </summary>
        public void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            Level++;
            Cost += _changeCost;
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }

        /// <summary>
        /// Sets the stat's level, value and cost.
        /// <p>Clamps the values to max level.</p>
        /// <p>Invokes OnStatUpdate event.</p>
        /// </summary>
        public void SetLevel(int level)
        {
            Level = level;
            if (IsMaxLevel()) Level = MaxLevel;
            Cost = _baseCost + (Level - 1) * _changeCost;
            OnStatUpdate?.Invoke(Level, Value, Cost);
        }
        
        protected bool IsMaxLevel()
        {
            return Level >= MaxLevel;
        }

        /// <summary>
        /// Makes the ShopItemButton for the shop.
        /// </summary>
        public ShopItemButton GetShopButton()
        {
            return new ShopItemButton(this, GetShopItemName(), "", GetCostString(), IsMaxLevel(), Cost);
        }
        
        private string GetShopItemName()
        {
            var shopItemNameBuilder = new StringBuilder();
            shopItemNameBuilder.Append(Name);
            if (!IsMaxLevel())
            {
                shopItemNameBuilder.Append(" ");
                shopItemNameBuilder.Append(Level);
            }
            return shopItemNameBuilder.ToString();
        }
        
        private string GetCostString()
        {
            return IsMaxLevel() ? "MAX LEVEL" : "$" + Cost;
        }

        public void LinkProgressBar(ProgressBar progressBar)
        {
            progressBar.SetMaxValue(0, Value, 0);
            OnStatUpdate += progressBar.SetMaxValue;
        }
    }
}