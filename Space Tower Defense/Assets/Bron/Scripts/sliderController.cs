using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sliderController : MonoBehaviour
{
    public TextMeshProUGUI valueText;
    public float newVolume;

    public void OnSliderChangedVolume(float value)
    {
        newVolume = value+80;
        valueText.text = newVolume.ToString("0") + '%';
    }

    public void OnSliderChangedSenes(float value)
    {
        PlayerCam.sensX = value;
        PlayerCam.sensY = value;
        valueText.text = value.ToString("0.00");
    }
}
