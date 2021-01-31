using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {MENU,APP }
public class GameManager : MonoBehaviour
{
    Bluetooth bluetooth;
    ColorPicker colorPicker;
    public ColorSelected[] colorsPalete;
    public Text bluetoothState;

    public GameObject[] Colors;
    public GameObject[] patterns;

   

    public GameObject mainPanel,appPanel;

    public GameState gameState;

    string dataToSend;
    private void Awake()
    {
        bluetooth = FindObjectOfType<Bluetooth>();
        colorPicker = FindObjectOfType<ColorPicker>();
        SetGameState(GameState.MENU);
    }
    public void ConnectBluetooth()
    {
        bluetooth.SetConnection();
        if (bluetooth.Connected)
        {
            bluetoothState.text = "Connected";
            SetGameState(GameState.APP);
        }
        else
        {
            bluetoothState.text = "Disconected";
            SetGameState(GameState.MENU);
        }
    }
   /// <summary>
   /// Sets the coshen color to the color palete and sends it via bluetooth to the arduino
   /// </summary>
   /// <param name="_pickedColor"></param>
    public void SetColorPalete(Color _pickedColor)
    {
        foreach (ColorSelected CS in colorsPalete)
        {
            if (CS.BeingPicked)
            {
                CS.color = _pickedColor;
                CS.spRenderer.color = _pickedColor;
                CS.anim.SetBool("BeingPicked", false);
                CS.BeingPicked = false;

                //Debug.Log("SELECTED R:" + CS.color.r + " G:" + CS.color.g + " B:" + CS.color.b);
                bluetooth.WriteColor(CS.color, CS.id);
                bluetooth.WrtiePattern(0);
            }
        }
    }

    public void GoToApp()
    {
        SetGameState(GameState.APP);
    }
    public void SetGameState( GameState _state)
    {
         switch(_state)
        {
            case GameState.MENU:
                mainPanel.SetActive(true);
                appPanel.SetActive(false);
                break;
            case GameState.APP:
                mainPanel.SetActive(false);
                appPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

}
