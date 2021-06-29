using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float timeToReach = 0;

    private float _timer;
    public float atBeat;

	private void OnEnable()
	{
        _timer = 0;
	}

	private void FixedUpdate()
	{
		if (pathCreator != null)
        {
            _timer += Time.deltaTime / timeToReach * pathCreator.path.length;
            transform.position = pathCreator.path.GetPointAtDistance(_timer, end);
        }
	}
}