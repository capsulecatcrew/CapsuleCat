using UnityEngine;

namespace Battle.Hitboxes
{
    public class ShieldKillable : Killable
    {
        [SerializeField] private Renderer colorRenderer;
        [SerializeField] private Color maxHpColor;
        [SerializeField] private Color minHpColor;
        
        public override void OnEnable()
        {
            base.OnEnable();
            OnDamaged += UpdateShieldColor;
        }
        
        private void UpdateShieldColor(float unused, DamageType unused2)
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