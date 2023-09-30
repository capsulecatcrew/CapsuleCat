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

    protected override void Awake()
    {
        onClick.AddListener(HandleClick);
    }

    public new void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        GlobalAudio.Singleton.PlaySound("UI_BTN_HIGHLIGHTED");
        OnHighlighted.Invoke();
    }
    private void HandleClick()
    {
        if (useable)
        {
            GlobalAudio.Singleton.PlaySound("UI_BTN_PRESSED");
            OnCustomPressed.Invoke();
        }
        else
        {
            GlobalAudio.Singleton.PlaySound("UI_BTN_DISABLED");
            OnDisabledTriggered.Invoke();
        }
    }

}
