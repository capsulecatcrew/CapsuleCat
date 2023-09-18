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
public class CapsuleCatBtn : Button, ISelectHandler
{
    [Header("Events")]
    public UnityEvent OnHighlighted;
    public UnityEvent OnCustomPressed;
    public UnityEvent OnDisabledTriggered;

    [SerializeField] private bool useable = true;

    private void Awake()
    {
        onClick.AddListener(HandleClick);
    }

    public new void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        GlobalAudio.Singleton.PlaySound("Healing"); // highlighted sound
        OnHighlighted.Invoke();
    }
    private void HandleClick()
    {
        if (useable)
        {
            GlobalAudio.Singleton.PlaySound("Healing"); // pressed sound
            OnCustomPressed.Invoke();
        }
        else
        {
            GlobalAudio.Singleton.PlaySound("Healing"); // disabled sound
            OnDisabledTriggered.Invoke();
        }
    }

}
