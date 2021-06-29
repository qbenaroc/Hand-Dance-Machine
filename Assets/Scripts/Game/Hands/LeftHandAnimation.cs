using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class LeftHandAnimation : MonoBehaviour
{
	private Animator _handAnimator;

	private bool _isFist = false;
	private bool _isFinger = false;

	private void Start()
	{
		_handAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		UpdateHandAnimation();
	}

	private void UpdateHandAnimation()
	{
		if (LeftHand._i._leftController.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
			_handAnimator.SetFloat("Trigger", triggerValue);
		else
			_handAnimator.SetFloat("Trigger", 0);
		if (LeftHand._i._leftController.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
			_handAnimator.SetFloat("Grip", gripValue);
		else
			_handAnimator.SetFloat("Grip", 0);
		if (triggerValue >= 0.9 && gripValue >= 0.9)
			_isFist = true;
		else
			_isFist = false;
		if (triggerValue == 0 && gripValue >= 0.9)
			_isFinger = true;
		else
			_isFinger = false;
	}
}
