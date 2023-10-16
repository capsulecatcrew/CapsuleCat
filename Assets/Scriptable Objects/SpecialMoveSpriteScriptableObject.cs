using System;
using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scriptable_Objects
{ 
    [CreateAssetMenu(fileName = "SpecialMoveSpriteScriptableObject", menuName = "Special Move Sprites")]
    public class SpecialMoveSpriteScriptableObject: ScriptableObject
    {
        [System.Serializable]
        public class SpecialMoveSprite
        {
            public Sprite mainSprite;
            public Sprite activatedSprite;
        }
        
        public SpecialMoveSprite healSprite;
        public SpecialMoveSprite absorbShieldSprite;
        public SpecialMoveSprite vampireSprite;
        public SpecialMoveSprite laserSprite;
        
        public Sprite noSpecialSprite;

        public Sprite GetSprite(SpecialMoveEnum specialMoveEnum, bool activated = false)
        {
            return specialMoveEnum switch
            {
                SpecialMoveEnum.MoveHeal => activated ? healSprite.activatedSprite
                                                      : healSprite.mainSprite,
                SpecialMoveEnum.MoveAbsorbShield => activated ? absorbShieldSprite.activatedSprite
                                                              : absorbShieldSprite.mainSprite,
                SpecialMoveEnum.ShootVampire => activated ? vampireSprite.activatedSprite
                                                          : vampireSprite.mainSprite,
                SpecialMoveEnum.ShootLaser => activated ? laserSprite.activatedSprite
                                                        : laserSprite.mainSprite,
                _ => noSpecialSprite
            };
        }

        public Sprite GetSprite(SpecialMove specialMove, bool activated = false)
        {
            return specialMove switch
            {
                Heal => activated ? healSprite.activatedSprite
                                  : healSprite.mainSprite,
                AbsorbShield => activated ? absorbShieldSprite.activatedSprite
                                          : absorbShieldSprite.mainSprite,
                Vampire => activated ? vampireSprite.activatedSprite
                                     : vampireSprite.mainSprite,
                Laser => activated ? laserSprite.activatedSprite
                                   : laserSprite.mainSprite,
                _ => noSpecialSprite
            };
        }
        
    }
}