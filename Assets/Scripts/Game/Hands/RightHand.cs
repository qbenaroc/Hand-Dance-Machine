using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class RightHand : MonoBehaviour
{
    public static RightHand _i;

    public InputDevice _rightController;
    private static List<InputDevice> _rightHandDevices = new List<InputDevice>();
    private static HapticCapabilities _cap = new HapticCapabilities();

	private void Awake()
    {
        _i = this;
        GetDevice();
    }

	private void Start()
	{
    }

	private void Update()
    {
        if (_rightHandDevices.Count == 0)
            this.GetDevice();
	}

    //Stock Right Controller
    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, _rightHandDevices);

        if (_rightHandDevices.Count == 1)
        {
            _rightController = _rightHandDevices[0];
            //Stock Right Controller Capabilities for Haptics
            _rightController.TryGetHapticCapabilities(out _cap);
        }
        else if (_rightHandDevices.Count > 1)
        {
            // Debug.LogError("Found more than one Left Hand !");
        }
        else if (_rightHandDevices.Count == 0)
        {
            // Debug.LogError("No Left Hand Found !");
        }
    }

    public void HapticImpulse(uint channel, float amplitude, float duration)
    {
        if (_rightController.isValid)
        {
            if (_cap.supportsImpulse)
                _rightController.SendHapticImpulse(channel, amplitude, duration);
        }
    }

    public void StopMyHaptics()
    {
        if (_rightController.isValid)
            _rightController.StopHaptics();
    }

	public void ValidHaptics()
	{
		HapticImpulse(0, 0.5f, 0.125f);
		HapticImpulse(0, 0.75f, 0.25f);
	}

	public void NonValidHaptics()
	{
		HapticImpulse(0, 0.75f, 0.125f);
		HapticImpulse(0, 0.25f, 0.25f);
	}
}