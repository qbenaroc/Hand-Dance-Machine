using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpeedShader : MonoBehaviour
{
    public int _band;
    private Renderer _rend;

    // Start is called before the first frame update
    void Start()
    {
        _rend = GetComponent<Renderer>();
        _rend.material.shader = Shader.Find("Shader Graphs/Lightspeed_SG");
    }

    // Update is called once per frame
    void Update()
    {
        _rend.material.SetFloat("Vector1_2A2F0F6C", 20f - AudioPeer.sharedInstance._amplitudeBuffer * 15f);
        //_rend.material.SetVector("Vector4_7B6883D0", new Vector4(-0.75f, -0.01f * AudioPeer._amplitudeBuffer, -0.7f, 3.43f));
    }
}