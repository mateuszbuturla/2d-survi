using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI label;

    public void SetupBar(float min, float max, float current)
    {
        slider.minValue = min;
        slider.maxValue = max;
        UpdateValue(current);
    }

    public void UpdateValue(float value)
    {
        slider.value = value;
        UpdateLabel((int)Math.Floor(value), slider.maxValue);
    }

    private void UpdateLabel(int value, float max)
    {
        label.text = $"{value}/{max}";
    }
}
