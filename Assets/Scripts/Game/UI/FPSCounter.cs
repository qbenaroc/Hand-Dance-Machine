using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
	[SerializeField] private bool _displayFps;
	TextMeshProUGUI fpsText;

    // Start is called before the first frame update
    void Start()
    {
		fpsText = GetComponent<TextMeshProUGUI>();  
    }

    // Update is called once per frame
    void Update()
    {
		if (_displayFps)
		{
			float current = 0;
			current = (int)(1f / Time.unscaledDeltaTime);
			fpsText.text = "FPS : " + current.ToString();
		}
    }
}
