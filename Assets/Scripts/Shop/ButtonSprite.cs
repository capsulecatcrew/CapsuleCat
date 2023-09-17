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

        _spriteRenderer.sprite = state switch
        {
            0 => normalSprite,
            1 => highlightedSprite,
            2 => disabledSprite,
            _ => _spriteRenderer.sprite
        };
    }

    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }
}
