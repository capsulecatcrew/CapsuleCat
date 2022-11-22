using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider barFill;
    
    // Start is called before the first frame update
    void Start()
    {
        Canvas.ForceUpdateCanvases();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMax(float value)
    {
        barFill.maxValue = value;
    }
    
    public void SetFill(float amount)
    {
        barFill.value = amount;
    }
}
