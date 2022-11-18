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

    public void SetMax(int value)
    {
        barFill.maxValue = value;
    }
    
    public void SetFill(int amount)
    {
        barFill.value = amount;
    }
}
