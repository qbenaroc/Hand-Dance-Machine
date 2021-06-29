using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    public Transform target;
    public float timeToReach = 4;

    private Vector3 _startPos;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime / timeToReach;
        
        if (transform.position == target.position)
		{
            transform.position = _startPos;
            _timer = 0;
        }
        else
            transform.position = Vector3.Lerp(_startPos, target.position, _timer);
    }
}