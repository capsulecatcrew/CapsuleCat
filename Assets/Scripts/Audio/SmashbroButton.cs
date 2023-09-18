using UnityEngine;

/// <summary>
/// Adds ui button sfx for the smash-bro buttons in shop scene
/// </summary>
public class SmashbroButton : Interactable
{
    [SerializeField] private ButtonSprite buttonSprite;

    private void Awake()
    {
        m_InteractEvent.AddListener(PlayPressedSound);
    }

    /// <summary>
    /// Called by Hitbox Trigger 2D Trigger Enter Event
    /// </summary>
    public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");
    public void PlayPressedSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_PRESSED");
    
    /// <summary>
    /// note: currently can't be played, disabling the button hasn't been implemented
    /// to implement, must hide the base InteractEvent
    /// </summary>
    public void PlayDisabledSound() => GlobalAudio.Singleton.PlaySound("UI_BTN_DISABLED");
    
}
