using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.sharedInstance.onSpacePressed += PrintSpace;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GameEvents.sharedInstance.SpacePressed();
    }

    void PrintSpace()
	{
        Debug.Log("Space !");
	}
}
