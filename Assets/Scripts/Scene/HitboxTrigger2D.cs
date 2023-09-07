using UnityEngine;
using UnityEngine.Events;

// This script mainly exists so not all logic has to be directly attached to the GameObject with the collider.
// Attach to a hitbox, have relevant components subscribe to its events.
public class HitboxTrigger2D : MonoBehaviour
{
    public string tagToTrigger;
    public UnityEvent m_TriggerEnterEvent;
    public UnityEvent m_TriggerStayEvent;
    public UnityEvent m_TriggerExitEvent;

    public delegate void HitboxEvent2D(Collider2D other);

    public event HitboxEvent2D HitboxEnter;
    
    public event HitboxEvent2D HitboxStay;

    public event HitboxEvent2D HitboxExit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tagToTrigger == "" || other.CompareTag(tagToTrigger))
        {
            HitboxEnter?.Invoke(other);
            m_TriggerEnterEvent.Invoke();
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (tagToTrigger == "" || other.CompareTag(tagToTrigger))
        {
            HitboxStay?.Invoke(other);
            m_TriggerStayEvent.Invoke();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (tagToTrigger == "" || other.CompareTag(tagToTrigger))
        {
            HitboxExit?.Invoke(other);
            m_TriggerExitEvent.Invoke();
        }
    }
}
