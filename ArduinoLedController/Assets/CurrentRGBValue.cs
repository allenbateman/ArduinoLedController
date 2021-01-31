using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentRGBValue : MonoBehaviour
{
    Text value;
    private void Start()
    {
        value = GetComponentInChildren<Text>();
    }

    public void OnValueChanged(float _value)
    {
        value.text = _value.ToString();
    }
}
