using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaternSelected : MonoBehaviour
{
    Bluetooth bluetooth;

    public float Delay;
    public int id;

    void Start()
    {
        bluetooth = FindObjectOfType<Bluetooth>();
    }
    public void SendPattern()
    {
        bluetooth.SendData("<pattern,"+id+","+Delay+">");
    }
}
