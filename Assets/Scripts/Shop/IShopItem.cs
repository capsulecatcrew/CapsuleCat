namespace Shop
{
    public interface IShopItem
    {
        public void Purchase();
        public string GetTitle();
        public string GetDescription();
        public int GetCost();
    }
}