using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected Slider slider;

    public void Start()
    {
        Canvas.ForceUpdateCanvases();
    }

    public void SetMaxValue(float maxValue)
    {
        slider.maxValue = maxValue;
    }
    
    public void HandleStatChange(float change)
    {
        if (slider.value + change > slider.maxValue)
        {
            slider.value = slider.maxValue;
        }
        else if (slider.value + change < 0)
        {
            slider.value = 0;
        }
        else
        {
            slider.value += change;
        }
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}
