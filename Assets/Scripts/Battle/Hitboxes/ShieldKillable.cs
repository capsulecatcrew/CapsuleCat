using UnityEngine;

namespace Battle.Hitboxes
{
    public class ShieldKillable : Killable
    {
        [SerializeField] private Renderer colorRenderer;
        [SerializeField] private Color maxHpColor;
        [SerializeField] private Color minHpColor;
        
        public override void Awake()
        {
            base.Awake();
            OnDamaged += UpdateShieldColor;
        }
        
        private void UpdateShieldColor(float ignored)
        {
            colorRenderer.material.color = GetDamageColor();
        }
    
        private Color GetDamageColor()
        {
            return Color.Lerp(minHpColor, maxHpColor, BattleStat.GetStatPercentage());
        }
        
        public override void OnDisable()
        {
            OnDamaged -= UpdateShieldColor;
        }
    }
}