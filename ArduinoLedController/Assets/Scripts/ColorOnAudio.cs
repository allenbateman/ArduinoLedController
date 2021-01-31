using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class ColorOnAudio : MonoBehaviour
{

    public int band;
    public float minIntensity, maxIntensity;
    Image sp;
    private AudioPeer audioPeer;
    public Color color;
    public Color displayColor = new Color(0,0,0);

    Bluetooth bluetooth;
    public int BAND_ID;

    void Start()
    {
        sp = GetComponent<Image>();
        sp.color = color;
        audioPeer = FindObjectOfType<AudioPeer>();
        bluetooth = FindObjectOfType<Bluetooth>();

    }
    private void Update()
    {
        UpdateColorOnAudio();
    }
    public void UpdateColorOnAudio() { 

        displayColor.r = Mathf.Lerp(0, color.r, (audioPeer.audioBandBuffer[band] * (maxIntensity - minIntensity)));
        displayColor.g = Mathf.Lerp(0, color.g, (audioPeer.audioBandBuffer[band] * (maxIntensity - minIntensity)));
        displayColor.b = Mathf.Lerp(0, color.b, (audioPeer.audioBandBuffer[band] * (maxIntensity - minIntensity)));
        sp.color = displayColor;

        Media();
    }

    public Color Media()
    {
        Color media = displayColor;
        media += displayColor / 2;
        return media;
    }

}
