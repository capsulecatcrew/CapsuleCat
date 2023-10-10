using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class SpecialIcon : MonoBehaviour
    {
        [SerializeField] private Image spriteRenderer;
        [SerializeField] private Sprite healSprite, absorbShieldSprite, vampireSprite, laserSprite;
        [SerializeField] private Sprite healEnabled, absorbShieldEnabled, vampireEnabled, laserEnabled;

        public void SetSprite(SpecialMove specialMove)
        {
            spriteRenderer.gameObject.SetActive(true);
            switch (specialMove)
            {
                case Heal:
                    spriteRenderer.sprite = healSprite;
                    return;
                case AbsorbShield:
                    spriteRenderer.sprite = absorbShieldSprite;
                    return;
                case Vampire:
                    spriteRenderer.sprite = vampireSprite;
                    return;
                case Laser:
                    spriteRenderer.sprite = laserSprite;
                    return;
                default:
                    spriteRenderer.gameObject.SetActive(false);
                    return;
            }
        }

        public void StartSpecial(SpecialMove specialMove)
        {
            switch (specialMove)
            {
                case Heal:
                    spriteRenderer.sprite = healEnabled;
                    return;
                case AbsorbShield:
                    spriteRenderer.sprite = absorbShieldEnabled;
                    return;
                case Vampire:
                    spriteRenderer.sprite = vampireEnabled;
                    return;
                case Laser:
                    spriteRenderer.sprite = laserEnabled;
                    return;
            }
        }

        public void StopSpecial(SpecialMove specialMove)
        {
            SetSprite(specialMove);
        }
    }
}