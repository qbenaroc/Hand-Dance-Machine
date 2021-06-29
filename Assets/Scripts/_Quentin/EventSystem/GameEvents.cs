using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents sharedInstance;

	public event Action onSpacePressed;
	public event Action<int> onItemDestroy;

	private void Awake() => sharedInstance = this;

	public void SpacePressed() => onSpacePressed?.Invoke();

	public void ItemDestroyed(int score)
	{
		onItemDestroy?.Invoke(score);
	}
}