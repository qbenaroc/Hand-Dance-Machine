using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class LeftHand : MonoBehaviour
{
    public static LeftHand _i;

	public InputDevice _leftController;
    private static List<InputDevice> _leftHandDevices = new List<InputDevice>();
    private static HapticCapabilities _cap = new HapticCapabilities();

	public void Awake()
    {
        _i = this;
        GetDevice();
    }

    public void Update()
    {
        if (_leftHandDevices.Count == 0)
            GetDevice();
	}

    //Stock Left Controller
    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, _leftHandDevices);

        if (_leftHandDevices.Count == 1)
        {
            _leftController = _leftHandDevices[0];
            //Stock Left Controller Capabilities for Haptics
            _leftController.TryGetHapticCapabilities(out _cap);
        }
        else if (_leftHandDevices.Count > 1)
        {
            // Debug.LogError("Found more than one Left Hand !");
        }
        else if (_leftHandDevices.Count == 0)
        {
            // Debug.LogError("No Left Hand Found !");
        }
    }

    public void HapticImpulse(uint channel, float amplitude, float duration)
    {
        if (_leftController.isValid)
        {
            if (_cap.supportsImpulse)
                _leftController.SendHapticImpulse(channel, amplitude, duration);
        }
    }

    public void StopMyHaptics()
    {
        if (_leftController.isValid)
        {
            _leftController.StopHaptics();
        }
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