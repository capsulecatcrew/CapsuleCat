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
        [SerializeField] private Sprite healSprite, absorbShieldSprite, vampireSprite;
        [SerializeField] private Sprite healEnabled, absorbShieldEnabled, vampireEnabled;

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
            }
        }

        public void StopSpecial(SpecialMove specialMove)
        {
            SetSprite(specialMove);
        }
    }
}