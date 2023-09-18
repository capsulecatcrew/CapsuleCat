using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Capsule Cat Button
/// Implements custom Disabled functionality and exposes OnHighlight event for sound fx
/// Do not used the native OnClick UnityEvent.
/// Plays button sound effects
///
/// Replaces default Unity Button
/// </summary>
public class CcButton : Button, ISelectHandler
{
    [Header("Events")]
    public UnityEvent OnHighlighted;
    public UnityEvent OnCustomPressed;
    public UnityEvent OnDisabledTriggered;

    [SerializeField] private bool customEnabled = true;

    private void Awake()
    {
        onClick.AddListener(HandleClick);
    }

    public void OnSelect(BaseEventData eventData)
    {
        GlobalAudio.Singleton.PlaySound(""); // highlighted sound
        OnHighlighted.Invoke();
    }
    private void HandleClick()
    {
        if (customEnabled)
        {
            GlobalAudio.Singleton.PlaySound(""); // pressed sound
            OnCustomPressed.Invoke();
        }
        else
        {
            GlobalAudio.Singleton.PlaySound(""); // disabled sound
            OnDisabledTriggered.Invoke();
        }
    }

}
