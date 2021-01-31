using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongPicker : MonoBehaviour
{
    public Dropdown dropdown;
    public List<AudioClip> Songs;
    Dropdown.OptionData optionData;
    private AudioPeer audioPeer;
    public Text currentSongPlaying;

    public Button playButton;
    bool playingSong;
    Bluetooth bluetooth;

    Image []sp;

    ColorOnAudio[] onAudios;


    private void Awake()
    {
        audioPeer = FindObjectOfType<AudioPeer>();
        bluetooth = FindObjectOfType<Bluetooth>();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Start()
    {

        dropdown.ClearOptions();
        onAudios = FindObjectsOfType<ColorOnAudio>();

        for (int i = 0; i < Songs.Count; i++)
        {
            optionData = new Dropdown.OptionData();
            optionData.text = Songs[i].name;
            dropdown.options.Add(optionData);
        }
        dropdown.onValueChanged.AddListener(delegate { SetPickedsong(); SetAudioButton();  });

        playingSong = false;
    }

    void SetPickedsong()
    {
        switch(dropdown.value)
        {
            case 0:
                audioPeer.SetClip(Songs[dropdown.value]);             
                break;
            case 1:
                audioPeer.SetClip(Songs[dropdown.value]);
                break;
            case 2:
                audioPeer.SetClip(Songs[dropdown.value]);
                break;
            case 3:
                audioPeer.SetClip(Songs[dropdown.value]);
                break;
            default:
                break;
        }
    }

    public void PlaySong()
    {
        if (!audioPeer.isAudioOn)
        {
            audioPeer.PlayAudio();
            StartCoroutine(UpdateColor());
        }
        else
        {
            audioPeer.StopAudio();
            StopAllCoroutines();
        }
        SetAudioButton();
    }
    void SetAudioButton()
    {
        if (audioPeer.isAudioOn)
        {
            bluetooth.WrtiePattern(5);
            currentSongPlaying.text = Songs[dropdown.value].name;
            playButton.GetComponentInChildren<Text>().text = "STOP";
        }
        else
        {
            playButton.GetComponentInChildren<Text>().text = "PLAY";
        }
    }
    IEnumerator UpdateColor ()
    {
        foreach(ColorOnAudio n in onAudios)
        {
            bluetooth.WriteColorBand(n.displayColor, n.BAND_ID);
            yield return new WaitForSeconds(0.1f);
        }


        yield return new WaitForSeconds(1f);
       // bluetooth.mySerialPort.DiscardOutBuffer();
        StartCoroutine(UpdateColor());
    }
    
}
