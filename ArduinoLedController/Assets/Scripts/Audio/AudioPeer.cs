using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{


    AudioSource AudioSource;
    AudioPeer AudioPeerInstance;
    public bool isAudioOn;

    float[] samplesLeft = new float[512];
    float[] samplesRight = new float[512];
    public float[] freqBand = new float[8];
    public float[] bandBuffer = new float[8];

    float[] freqBandHighest = new float[8];
    public float[] audioBand = new float[8];
    public float[] audioBandBuffer = new float[8];

    public static float Amplitude, AmplitudeBuffer;
    float AmplitudeHighest;
    public float audioProfile;
    float[] bufferDecrease = new float[8];

    public enum channel { Setero, Left, Right };
    public channel Channel = new channel();

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        AudioProfile(audioProfile);
        if (AudioPeerInstance != null)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            AudioPeerInstance = this;
        }
        isAudioOn = false;
    }
    private void OnEnable()
    {
        if(AudioSource != null)
        AudioSource.Stop();
    }
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }
    void AudioProfile(float _audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            freqBandHighest[i] = _audioProfile;
        }
    }
    void GetAmplitude()
    {
        float currentAmplitude = 0;
        float currentAmplitudeBuffer = 0;
        for (int i = 0; i < 8; i++)
        {
            currentAmplitude += audioBand[i];
            currentAmplitudeBuffer += audioBandBuffer[i];

        }
        if (currentAmplitude > AmplitudeHighest)
        {
            AmplitudeHighest = currentAmplitude;
        }
        Amplitude = currentAmplitude / AmplitudeHighest;
        AmplitudeBuffer = currentAmplitudeBuffer / AmplitudeHighest;
    }
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBand[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBand[i];

            }
            audioBand[i] = (freqBand[i] / freqBandHighest[i]);
            audioBandBuffer[i] = (bandBuffer[i] / freqBandHighest[i]);

        }
    }
    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            bandBuffer[g] = freqBand[g];
            bufferDecrease[g] = 0.005f;

            if (freqBand[g] < bandBuffer[g])
            {
                bandBuffer[g] -= bufferDecrease[g];
                bufferDecrease[g] *= 1.2f;
            }
        }
    }
    void GetSpectrumAudioSource()
    {
        AudioSource.GetSpectrumData(samplesLeft, 0, FFTWindow.Blackman);
        AudioSource.GetSpectrumData(samplesRight, 1, FFTWindow.Blackman);

    }
    void MakeFrequencyBands()
    {
        /*
         * SONG HERTZS+?
            +22050 / 512 = 43 Herz per Sample
            20-60 h
            60 -250 herzt
            500-2000 hertzs
            2000-4000 herzs
            4000 - 6000 hertzs
            6000 - 2000 herzts

        0 - 2 = 88 herzts - RANGE
        1 - 4 = 172 hertzs - 87 - 258
        2 - 8 =  344 herzts - 259 - 602
        3 - 16 = 600 herzts -603-1290
        4 - 32 = 1376 herzts -1291 -2666
        5 - 64 = 2752 - 2667 - 5418
        6 -128 = 5504 herzts - 5419 - 10922
        7 - 256 = 11008 hertz - 10923 - 21930
        
        510
        */

        int count = 0;


        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2; // -MATH- ;S

            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (Channel == channel.Setero)
                {
                    average += samplesLeft[count] + samplesRight[count] * (count + 1);
                }
                if (Channel == channel.Right)
                {
                    average += samplesRight[count] * (count + 1);
                }
                if (Channel == channel.Left)
                {
                    average += samplesLeft[count] * (count + 1);
                }
                count++;
            }

            average /= count;

            freqBand[i] = average * 10;
        }


    }

    public void SetClip(AudioClip _clip)
    {
        AudioSource.clip = _clip;
        isAudioOn = false;
    }
    public void StopAudio()
    {
        AudioSource.Stop();
        isAudioOn = false;
    }
    public void PlayAudio()
    {      
       AudioSource.Play();
       isAudioOn = true;
    }

}

