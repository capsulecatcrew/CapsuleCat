using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// This script mainly exists so not all logic has to be directly attached to the GameObject with the collider.
// Attach to a hitbox, have relevant components subscribe to its events.
public class HitboxTrigger2D : MonoBehaviour
{
    public List<string> tagsToTrigger;
    public UnityEvent m_TriggerEnterEvent;
    public UnityEvent m_TriggerStayEvent;
    public UnityEvent m_TriggerExitEvent;

    public delegate void HitboxEvent2D(Collider2D other);

    public event HitboxEvent2D HitboxEnter;
    
    public event HitboxEvent2D HitboxStay;

    public event HitboxEvent2D HitboxExit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tagsToTrigger.Count > 0 && !tagsToTrigger.Contains(other.tag)) return;
        HitboxEnter?.Invoke(other);
        m_TriggerEnterEvent.Invoke();
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (tagsToTrigger.Count > 0 && !tagsToTrigger.Contains(other.tag)) return;
        HitboxStay?.Invoke(other);
        m_TriggerStayEvent.Invoke();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (tagsToTrigger.Count > 0 && !tagsToTrigger.Contains(other.tag)) return;
        HitboxExit?.Invoke(other);
        m_TriggerExitEvent.Invoke();
    }
}
