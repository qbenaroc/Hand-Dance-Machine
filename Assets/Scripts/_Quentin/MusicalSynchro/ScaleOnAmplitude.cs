using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool _useBuffer;
    private Material _material;
    public float _red, _green, _blue;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        if (!_useBuffer)
        {
            transform.localScale = new Vector3((AudioPeer.sharedInstance._amplitude * _maxScale) + _startScale, (AudioPeer.sharedInstance._amplitude * _maxScale) + _startScale
                                            , (AudioPeer.sharedInstance._amplitude * _maxScale) + _startScale);
            Color _color = new Color(_red * AudioPeer.sharedInstance._amplitude, _green * AudioPeer.sharedInstance._amplitude, _blue * AudioPeer.sharedInstance._amplitude);
            _material.SetColor("_EmissionColor", _color);
        }
        else
        {
            transform.localScale = new Vector3((AudioPeer.sharedInstance._amplitudeBuffer * _maxScale) + _startScale, (AudioPeer.sharedInstance._amplitudeBuffer * _maxScale) + _startScale
                                            , (AudioPeer.sharedInstance._amplitudeBuffer * _maxScale) + _startScale);
            Color _color = new Color(_red *  AudioPeer.sharedInstance._amplitudeBuffer, _green * AudioPeer.sharedInstance._amplitudeBuffer, _blue * AudioPeer.sharedInstance._amplitudeBuffer);
            _material.SetColor("_EmissionColor", _color);
        }
    }
}
