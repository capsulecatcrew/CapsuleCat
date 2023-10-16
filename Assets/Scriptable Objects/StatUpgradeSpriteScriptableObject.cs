using Player.Stats.Templates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "StatUpgradeSpriteScriptableObject", menuName = "Stat Sprites")]
    public class StatUpgradeSpriteScriptableObject: ScriptableObject
    {
        public Sprite maxHealth;
        public Sprite attackDamage;
        public Sprite maxEnergy;
        public Sprite energyAbsorb;
        public Sprite energyShare;
        public Sprite dashEnergyCost;
        public Sprite specialGain;
        public Sprite locked;
        public Sprite GetSprite(UpgradeableStat stat)
        {
            return stat.Name switch
            {
                "Max Health" => maxHealth,
                "Attack Damage" => attackDamage,
                "Max Energy" => maxEnergy,
                "Energy Absorb" => energyAbsorb,
                "Energy Share" => energyShare,
                "Dash Energy Cost" => dashEnergyCost,
                "Special Gain" => specialGain,
                _ => locked
            };
        }

        public Sprite GetLockedSprite() => locked;
        
    }
}