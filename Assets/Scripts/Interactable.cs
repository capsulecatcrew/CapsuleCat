using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent<int> m_InteractWithInt;
    public UnityEvent m_InteractEvent;

    public void Interact()
    {
        m_InteractEvent.Invoke();
    }


    public void InteractWithInt(int x)
    {
        m_InteractWithInt.Invoke(x);
    }
    
}
