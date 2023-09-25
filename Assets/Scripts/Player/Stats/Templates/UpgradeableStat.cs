using System.Text;

namespace Player.Stats.Templates
{
    public class UpgradeableStat : Stat
    {
        protected int Level = 1;
        protected internal readonly int MaxLevel;
        
        protected readonly float ChangeValue;

        private int _cost;
        private readonly int _baseCost;
        private readonly int _changeCost;

        public delegate void StatUpdate(int level, float value, int cost);
        public event StatUpdate OnStatUpdate;
        
        protected UpgradeableStat(
            string name, int maxLevel, float baseValue, float changeValue, int baseCost, int changeCost, bool isHealthStat) :
            base (name, baseValue, isHealthStat)
        {
            MaxLevel = maxLevel;
            ChangeValue = changeValue;
            _cost = baseCost;
            _baseCost = baseCost;
            _changeCost = changeCost;
        }
        
        /// <summary>
        /// Resets the stat's level to 1.
        /// <p>Resets the stat's value to base value.</p>
        /// <p>Resets the stat's cost to base cost.</p>
        /// <p>Invokes OnStatReset, OnStatUpdate events.</p>
        /// </summary>
        public override void Reset()
        {
            Level = 1;
            _cost = _baseCost;
            base.Reset();
            OnStatUpdate?.Invoke(Level, Value, _cost);
        }
        
        /// <summary>
        /// Upgrades the stat's level, value and cost.
        /// <p> Does not upgrade if stat is at max level.</p>
        /// <p>Invokes OnStatUpdate event if not at max level.</p>
        /// </summary>
        public virtual void UpgradeLevel()
        {
            if (IsMaxLevel()) return;
            Level++;
            _cost += _changeCost;
            OnStatUpdate?.Invoke(Level, Value, _cost);
        }

        /// <summary>
        /// Sets the stat's level, value and cost.
        /// <p>Clamps the values to max level.</p>
        /// <p>Invokes OnStatUpdate event.</p>
        /// </summary>
        public virtual void SetLevel(int level)
        {
            Level = level;
            if (IsMaxLevel()) Level = MaxLevel;
            _cost = _baseCost + (Level - 1) * _changeCost;
            OnStatUpdate?.Invoke(Level, Value, _cost);
        }
        
        protected bool IsMaxLevel()
        {
            return Level >= MaxLevel;
        }

        /// <summary>
        /// Initialised a ShopItemButton in the shop.
        /// </summary>
        public void InitShopItemButton(ShopItemButton shopItemButton)
        {
            shopItemButton.Init(this, GetShopItemName(), GetCostString(), !IsMaxLevel(), _cost);
        }
        
        private string GetShopItemName()
        {
            var shopItemNameBuilder = new StringBuilder();
            shopItemNameBuilder.Append(Name);
            if (IsMaxLevel()) return shopItemNameBuilder.ToString();
            shopItemNameBuilder.Append(" ");
            shopItemNameBuilder.Append(Level);
            return shopItemNameBuilder.ToString();
        }
        
        private string GetCostString()
        {
            return IsMaxLevel() ? "MAX LEVEL" : "$" + _cost;
        }

        public void InitProgressBar(ProgressBar progressBar)
        {
            progressBar.SetMaxValue(0, Value, 0);
        }
    }
}