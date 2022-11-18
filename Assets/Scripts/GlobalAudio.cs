using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AudioSource))]
public class GlobalAudio : MonoBehaviour
{
    public static AudioSource AudioSource;
    // Start is called before the first frame update
    void Awake()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
