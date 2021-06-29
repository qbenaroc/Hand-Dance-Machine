using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchroVertices : MonoBehaviour
{
    public float maxScale;
    public float startScale = 0;

    Mesh _mesh;
    Vector3[] _vertices;
    int[] _band;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _vertices = _mesh.vertices;
        _band = new int[_vertices.Length];

        for (int i = 0; i < _vertices.Length; i++)
        {
            _band[i] = Random.Range(0, AudioPeer.sharedInstance._bandBuffer.Length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < _vertices.Length; i++)
		{
            //if (i != 0 && i % 10 == 0)
                _vertices[i].y = (AudioPeer.sharedInstance._bandBuffer[_band[i]] * maxScale) + startScale;
		}
        _mesh.vertices = _vertices;
        _mesh.RecalculateBounds();
    }
}