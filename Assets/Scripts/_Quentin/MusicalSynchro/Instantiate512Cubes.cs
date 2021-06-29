using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate512Cubes : MonoBehaviour
{
    public GameObject _sampleCubePrefab;
    private GameObject[] _sampleCube = new GameObject[512];
    public float _maxScale;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 512; i++)
        {
            GameObject _instantiateSampleCube = Instantiate(_sampleCubePrefab);
            _instantiateSampleCube.transform.position = transform.position;
            _instantiateSampleCube.transform.parent = transform;
            _instantiateSampleCube.name = "SampleCube" + i;
            transform.eulerAngles = new Vector3(0, -0.703125f * i, 0);
            _instantiateSampleCube.transform.position = Vector3.forward * 100;
            _sampleCube[i] = _instantiateSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 512; i++)
        {
            if (_sampleCube != null)
            {
                _sampleCube[i].transform.localScale = new Vector3(10, ((AudioPeer._samplesLeft[i] + AudioPeer._samplesRight[i]) * _maxScale) + 2, 10);
            }
        }
    }
}
