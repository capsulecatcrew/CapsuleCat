using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected Slider slider;
    
    void Start()
    {
        Canvas.ForceUpdateCanvases();
    }

    protected void SetMaxValue(int ignore1, float maxValue, int ignore2)
    {
        slider.maxValue = maxValue;
    }
    
    protected void SetValue(float value)
    {
        slider.value = value;
    }

    protected void AddValue(float value)
    {
        slider.value += value;
    }

    protected void MinusValue(float value)
    {
        slider.value -= value;
    }
}
