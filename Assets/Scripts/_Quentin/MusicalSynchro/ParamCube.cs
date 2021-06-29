using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int _band;
    public float _startScale, _maxScale;
    public bool _useBuffer;
    public bool _normalize;
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (_useBuffer)
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.sharedInstance._bandBuffer[_band] * _maxScale) + _startScale, transform.localScale.z);
        else if (_normalize)
        {
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.sharedInstance._audioBandBuffer[_band] * _maxScale) + _startScale, transform.localScale.z);
            Color _color = new Color(AudioPeer.sharedInstance._audioBandBuffer[_band], AudioPeer.sharedInstance._audioBandBuffer[_band], AudioPeer.sharedInstance._audioBandBuffer[_band]);
            _material.SetColor("_EmissionColor", _color);
        }
        else
            transform.localScale = new Vector3(transform.localScale.x, (AudioPeer.sharedInstance._freqBand[_band] * _maxScale) + _startScale, transform.localScale.z);
    }
}
