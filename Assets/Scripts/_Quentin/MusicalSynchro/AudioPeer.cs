using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource _audioSource;

    public static AudioPeer sharedInstance;

    public static float[] _samplesLeft = new float[512];
    public static float[] _samplesRight = new float[512];

    [Header("Frequencies Treatment")]
    public float[] _freqBand = new float[8];
    public float[] _bandBuffer = new float[8];

    public float[] _freqBandHighest = new float[8];
    public float[] _audioBand = new float[8];
    public float[] _audioBandBuffer = new float[8];

    public float _amplitude, _amplitudeBuffer;
    private float _amplitudeHighest;

    public float _audioHighestProfile;

    private float[] _bufferDecrease = new float[8];
    [Header("Buffer Values")]
    public float _bufferDecreaseAmount = 0.005f;
    public float _bufferDecreaseMultiplier = 1.2f;

    public enum _channel {Stereo, Left, Right};
    public _channel channel = new _channel();

	private void Awake()
	{
        sharedInstance = this;
	}

	void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        SetAudioHighestProfile(_audioHighestProfile);
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    void SetAudioHighestProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void GetAmplitude()
    {
        float _currentAmplitude = 0;
        float _currentAmplitudeBuffer = 0;

        for (int i = 0; i < 8; i++)
        {
            _currentAmplitude += _audioBand[i];
            _currentAmplitudeBuffer += _audioBandBuffer[i];
        }
        if (_currentAmplitude > _amplitudeHighest)
            _amplitudeHighest = _currentAmplitude;
        _amplitude = _currentAmplitude / _amplitudeHighest;
        _amplitudeBuffer = _currentAmplitudeBuffer / _amplitudeHighest;
        if (float.IsNaN(_amplitude))
            _amplitude = 0;
        if (float.IsNaN(_amplitudeBuffer))
            _amplitudeBuffer = 0;
    }

    //Float between 0 and 1
    void CreateAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
                _freqBandHighest[i] = _freqBand[i];
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    //Smoothed Frequencies
    void BandBuffer()
    {
        for (int g = 0; g < 8; g++)
        {
            if (_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = _bufferDecreaseAmount;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= _bufferDecreaseMultiplier;
            }
        }
    }

    //Divide Frequencies in 8 Bands 
    void MakeFrequencyBands()
    {
        /* 0 - 2 samples ~= 20 - 60 hertz
         * 1 - 4 samples ~= 20 - 60 hertz
         * 2 - 8 samples ~= 60 - 250 hertz
         * 3 - 16 samples ~= 250 - 500 hertz
         * 4 - 32 samples ~= 500 - 2000 hertz
         * 5 - 64 samples ~= 2000 - 4000 hertz
         * 6 - 128 samples ~= 4000 - 6000 hertz
         * 7 - 256 samples ~= 6000 - 20000 hertz
         * Total 510 ajout 2 à la fin
         */

        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            if (i == 7)
                sampleCount += 2;
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                    average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                else if (channel == _channel.Left)
                    average += _samplesLeft[count] * (count + 1);
                else if (channel == _channel.Right)
                    average += _samplesRight[count] * (count + 1);
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }
    }
}