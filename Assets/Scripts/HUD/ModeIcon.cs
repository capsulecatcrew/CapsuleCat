using UnityEngine;
using UnityEngine.UI;

public class ModeIcon : MonoBehaviour
{
    public Image spriteRenderer;
    public Sprite movementSprite, shootingSprite;

    public void SetSprite(ControlMode mode)
    {
        switch (mode)
        {
            case ControlMode.Movement:
                spriteRenderer.sprite = movementSprite;
                break;
            case ControlMode.Shooting:
                spriteRenderer.sprite = shootingSprite;
                break;
            default:
                break;
        }
    }

}
