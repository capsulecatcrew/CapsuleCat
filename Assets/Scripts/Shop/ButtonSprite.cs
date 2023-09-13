using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ButtonSprite : MonoBehaviour
{
    public bool useable = true;
    [Header("Sprite Properties")]
    private SpriteRenderer _spriteRenderer;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public Sprite disabledSprite;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Sets sprite to different states.
    /// Will only work if button is not disabled.
    /// 
    /// States:
    /// 0 - normal
    /// 1 - highlighted
    /// 2 - disabled
    /// </summary>
    /// <param name="state">state number</param>
    public void SetToSpriteState(int state)
    {
        if (!useable) return;

        switch (state)
        {
            case 0:
                _spriteRenderer.sprite = normalSprite;
                break;
            case 1:
                _spriteRenderer.sprite = highlightedSprite;
                break;
            case 2:
                _spriteRenderer.sprite = disabledSprite;
                break;
            default:
                break;
        }
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
