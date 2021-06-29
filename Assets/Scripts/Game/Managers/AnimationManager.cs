using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	public static AnimationManager _i { get; private set; }

    public enum Anim
	{
		Ok,
		Perfect,
		Bad,
	}

	private void Awake()=> _i = this;

	public void PlayAnim(Anim anim, Vector3 position)
	{
		GameObject oneShotAnim = Instantiate(GetAnimation(anim), position, Quaternion.identity);
		Animator oneShotAnimator = oneShotAnim.GetComponent<Animator>();

		//oneShotAnimator.Play("Scene", 0, 0f);
		oneShotAnim.transform.parent = transform;
		Destroy(oneShotAnim, oneShotAnimator.GetCurrentAnimatorStateInfo(0).length);
	}

	private GameObject GetAnimation(Anim anim)
	{
		foreach (AnimationClip animationClip in AssetsManager.sharedInstance.animationClipArray)
		{
			if (animationClip.anim == anim)
			{
				return animationClip.animPrefab;
			}
		}
		Debug.LogError("Animation" + anim + " not found");
		return null;
	}
}