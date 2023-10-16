using Player.Special;
using Player.Special.Move;
using Player.Special.Shoot;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace HUD
{
    public class SpecialIcon : MonoBehaviour
    {
        [SerializeField] private Image spriteRenderer;
        [SerializeField] private SpecialMoveSpriteScriptableObject sprites;

        public void SetSprite(SpecialMove specialMove)
        {
            spriteRenderer.gameObject.SetActive(true);
            switch (specialMove)
            {
                // TODO: any idea how to shorten this?
                case Heal:
                    spriteRenderer.sprite = sprites.GetSprite(specialMove);
                    return;
                case AbsorbShield:
                    spriteRenderer.sprite = sprites.GetSprite(specialMove);
                    return;
                case Vampire:
                    spriteRenderer.sprite = sprites.GetSprite(specialMove);
                    return;
                case Laser:
                    spriteRenderer.sprite = sprites.GetSprite(specialMove);
                    return;
                default:
                    spriteRenderer.gameObject.SetActive(false);
                    return;
            }
        }

        public void StartSpecial(SpecialMove specialMove)
        {
            spriteRenderer.sprite = sprites.GetSprite(specialMove, true);
        }

        public void StopSpecial(SpecialMove specialMove)
        {
            SetSprite(specialMove);
        }
    }
}