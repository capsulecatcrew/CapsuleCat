using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotation : MonoBehaviour
{
    public bool rotate = true;

    public float rotateSpeed = 5;

    public bool verticalOscillate = true;

    public float dist = 0.5f;

    private float _time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= Mathf.PI * 2) _time = 0;
        if (verticalOscillate) transform.Translate(0, dist * Mathf.Cos(_time) * Time.deltaTime, 0, Space.World);
        if (rotate) transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
    }
}
