using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script mainly exists so not all logic has to be directly attached to the GameObject with the collider.
// Attach to a hitbox, have relevant components subscribe to its events.
public class HitboxTrigger : MonoBehaviour
{
    public delegate void HitboxEvent(Collider other);

    public event HitboxEvent HitboxEnter;
    
    public event HitboxEvent HitboxStay;

    public event HitboxEvent HitboxExit;
    
    private void OnTriggerEnter(Collider other)
    {
        HitboxEnter?.Invoke(other);
    }
    
    private void OnTriggerStay(Collider other)
    {
        HitboxStay?.Invoke(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        HitboxExit?.Invoke(other);
    }
}
