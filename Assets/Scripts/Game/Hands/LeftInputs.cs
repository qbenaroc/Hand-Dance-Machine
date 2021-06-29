using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(LeftHand))]
public class LeftInputs : MonoBehaviour
{
    public static LeftInputs _i;

    [HideInInspector] public bool menuButtonState;
    
    private Vector2 axisValue;
    private InputDevice _lController;

	private void Awake()
	{
        _i = this;
    }

	// Start is called before the first frame update
	void Start()
    {
        _lController = LeftHand._i._leftController;
        menuButtonState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_lController.TryGetFeatureValue(CommonUsages.menuButton, out menuButtonState))
        {
            if (menuButtonState)
            {
                Debug.Log("Pause");
                TimeControl._i.PauseGame();
            }
        }
        if (_lController.TryGetFeatureValue(CommonUsages.primary2DAxis, out axisValue))
		{
            if (TimeControl._i.gameIsPaused)
            {
                if (axisValue.x == 0)
                    TimeControl._i.StopGameTime();
                if (axisValue.x > 0)
                    TimeControl._i.ForwardGame(axisValue.x);
                if (axisValue.x < 0)
                    TimeControl._i.BackwardGame(axisValue.x);
            }
		}
    }
}