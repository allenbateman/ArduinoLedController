using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FunctionsController : MonoBehaviour
{
    
    private Bluetooth bluetooth;
    public Slider brightnessSlider, speedSlider;
    public Button function1, function2, function3, function4;
    void Start()
    {
        brightnessSlider.maxValue = 255;
        brightnessSlider.minValue = 0;
        brightnessSlider.value = 255;

        speedSlider.maxValue = 100;
        speedSlider.minValue = 5;
        speedSlider.value = 50;

        bluetooth = FindObjectOfType<Bluetooth>();

        speedSlider.onValueChanged.AddListener(delegate { bluetooth.WriteSpeed((int)speedSlider.value); });
        brightnessSlider.onValueChanged.AddListener(delegate { bluetooth.WriteBrightness((int)brightnessSlider.value); });
        function1.onClick.AddListener(delegate { bluetooth.WrtiePattern(1); });//pingpong
        function2.onClick.AddListener(delegate { bluetooth.WrtiePattern(2); });//BlinckBlinck
        function3.onClick.AddListener(delegate { bluetooth.WrtiePattern(3); });//strip animation
        function4.onClick.AddListener(delegate { bluetooth.WrtiePattern(4); });//gradient change

    }
}
