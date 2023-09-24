using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected Slider slider;

    public void Start()
    {
        Canvas.ForceUpdateCanvases();
    }

    public void SetMaxValue(int ignored1, float maxValue, int ignored2)
    {
        slider.maxValue = maxValue;
    }
    
    public void SetValue(float value)
    {
        slider.value = value;
    }
}
