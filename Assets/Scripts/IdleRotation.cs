using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotation : MonoBehaviour
{
    public bool rotate = true;

    public float rotateSpeed = 5;

    public bool verticalOscillate = true;

    public float dist = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (rotate) transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }
}
