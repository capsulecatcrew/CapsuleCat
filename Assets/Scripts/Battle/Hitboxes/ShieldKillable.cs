using UnityEngine;

namespace Battle.Hitboxes
{
    public class ShieldKillable : Killable
    {
        [SerializeField] private Renderer colorRenderer;
        [SerializeField] private Color maxHpColor;
        [SerializeField] private Color minHpColor;
        
        public void OnEnable()
        {
            OnHitBox += UpdateShieldColor;
        }
        
        private void UpdateShieldColor(float ignored1, DamageType ignored2)
        {
            colorRenderer.material.color = GetDamageColor();
        }
    
        private Color GetDamageColor()
        {
            return Color.Lerp(minHpColor, maxHpColor, BattleStat.GetStatPercentage());
        }
        
        public override void OnDisable()
        {
            OnHitBox -= UpdateShieldColor;
        }
    }
}