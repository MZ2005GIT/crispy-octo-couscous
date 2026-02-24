using UnityEngine;
using UnityEngine.UI;
public class HealthbarTemp : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float max)
    {
        slider.value = max;
        slider.maxValue = max;
    }
    public void SetHealth(float current)
    {
        slider.value = current;
    }
}
