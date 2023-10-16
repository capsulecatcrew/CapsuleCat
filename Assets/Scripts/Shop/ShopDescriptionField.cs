using Player.Special;
using Player.Stats.Templates;
using TMPro;
using UnityEngine;

namespace Shop
{
    public class ShopDescriptionField : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text costText;
    
        // Start is called before the first frame update
        private void Start()
        {
            titleText.text = "";
            descriptionText.text = "";
            if (costText != null) costText.text = "";
        }

        public void UpdateDescription(IShopItem item, bool purchased = false)
        {
            titleText.text = item.GetTitle();
            descriptionText.text = item.GetDescription();
            if (costText != null)
            {
                costText.text = purchased ? "BOUGHT" : "$" + item.GetCost();
            }
        }
    
    }
}
