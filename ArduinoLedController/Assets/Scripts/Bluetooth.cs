using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Text;
using HSVPicker;
public class Bluetooth : MonoBehaviour {
    public SerialPort mySerialPort;
    public Thread serialThread;
    public  int baudRate = 2000000;
    int readTimeOut = 200;
    int bufferSize = 128;

    char[] recivedChars;
    int[] recivedBytes;

    float frequency;
    [HideInInspector]
    public float incomingDistance;

    public bool Connected;

    void Start()
    {
        mySerialPort = new SerialPort("COM5", baudRate);
        recivedBytes = new int[bufferSize];
        recivedChars = new char[bufferSize];

  
    }
    public void SetConnection()
    {
        if (mySerialPort != null && Connected)
        {
            CloseConnection();
            Debug.Log("Disconnection Succed");
     
        }
        else if (mySerialPort != null && !Connected)
        {
            Connect();         
        }
    }
    public void Connect()
    {
        Debug.Log("Connection started");
        try
        {

            mySerialPort.BaudRate = baudRate;
            mySerialPort.ReadTimeout = readTimeOut;
            mySerialPort.WriteBufferSize = bufferSize;
            mySerialPort.DtrEnable = true;
            mySerialPort.RtsEnable = true;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            mySerialPort.Handshake = Handshake.RequestToSend;
            Connected = true;
            Debug.Log("Port Opened!");
        }
        catch (SystemException e)
        {
            Connected = false;
            Debug.Log("Error opening = " + e.Message);
        }
    }
    void Readbuffer()
    {
        if (mySerialPort != null && mySerialPort.IsOpen)
        {
            try
            {
                float.TryParse(mySerialPort.ReadLine(), out incomingDistance);
                Debug.Log("UltraSonic Sensor: " + incomingDistance);

            }
            catch (TimeoutException e)
            {
                Debug.LogWarning(e.ToString());
            }
        }

    }
    public static void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string indata = sp.ReadExisting();
        Debug.Log("Data Received:" + indata);
    }
    //manda un string con el mensaje, con writeLine escribimos en el puerto del arduino,
    //como si lo escribiesemos desde su consola
    public void SendData(string msg)
    {

        byte[]  bytes = mySerialPort.Encoding.GetBytes(msg);
        bufferSize = bytes.Length * sizeof(byte);
        Debug.Log("buffer size:" + bytes.Length * sizeof(byte) + " = "+ Encoding.ASCII.GetString(bytes));


        if (mySerialPort != null && mySerialPort.IsOpen)
        {
            try
            {
              //  mySerialPort.Write(bytes,0, bufferSize);
                mySerialPort.WriteLine(msg);
             //   mySerialPort.DiscardOutBuffer();
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }
    //cierra la conexión del puerto
    public void CloseConnection()
    {
        //control de errores 
        if (mySerialPort != null && mySerialPort.IsOpen)
        {
            try
            {
                mySerialPort.Close();
                Connected = false;
            }
            catch (System.Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }

    public void WriteColor(Color _color, int _id)
    {
        string msgToSend = "<|" + (int)(_color.r * 255 )+ "," + (int)(_color.g * 255) + "," + (int)(_color.b * 255) + "," + _id + ">";
        //Debug.Log("Color Sent: " + msgToSend);
        SendData(msgToSend);
    }
    public void WriteColorBand(Color _color,int _id)
    {
        string msgToSend = "<|" + (int)(_color.r * 255) + "," + (int)(_color.g * 255)  + "," + (int)(_color.b * 255 )+ "," + _id + ">";
       // Debug.Log("Color Sent: " + msgToSend);
        SendData(msgToSend);
    }
    public void WriteHSV(int H,int S,int V, int _id)
    {
        string msgToSend = "<|" + H * 255 + "," + S * 255 + "," + V * 255 + "," + _id + ">";
        SendData(msgToSend);
    }
    public void WriteBrightness(int _brightness)
    {
        string msgToSend = "<B"+_brightness+">";
       // Debug.Log("B Sent: " + msgToSend);
        SendData(msgToSend);
    }
    public void WrtiePattern(int _pattern)
    {
        string msgToSend = "<P" + _pattern + ">";
        SendData(msgToSend);
    }
    public void WriteSpeed(float _speed)
    {
        string msgToSend = "<S" + _speed + ">";
        SendData(msgToSend);
    }
    public void SendFrequency(float _frequency)
    {
        string msgToSend = "<F" + _frequency + ">";
        SendData(msgToSend);
    }
}
