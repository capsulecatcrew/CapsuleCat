using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class InteractZone : MonoBehaviour
{
    public delegate void NoInput();
    
    public event NoInput OnInteract;    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Input.GetKey("Submit")/*Player Inputs Submit*/)
        {
            OnInteract?.Invoke();
            Debug.Log("Selected");
        }
    }
}
