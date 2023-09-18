using UnityEngine;

/// <summary>
/// Adds ui button sfx for the smash-bro buttons in shop scene
/// </summary>
public class UiButtonSound : MonoBehaviour
{
    public void PlayHighlightedSound() => GlobalAudio.Singleton.PlaySound("");
    public void PlayPressedSound() => GlobalAudio.Singleton.PlaySound("");
    public void PlayDisabledSound() => GlobalAudio.Singleton.PlaySound("");
}
