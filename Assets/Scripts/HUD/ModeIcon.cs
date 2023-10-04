using UnityEngine;
using UnityEngine.UI;

public class ModeIcon : MonoBehaviour
{
    [SerializeField] private Image spriteRenderer;
    [SerializeField] private Sprite movementSprite, shootingSprite;

    public void SetSprite(ControlMode mode)
    {
        spriteRenderer.sprite = mode switch
        {
            ControlMode.Movement => movementSprite,
            ControlMode.Shooting => shootingSprite,
            _ => spriteRenderer.sprite
        };
    }
}