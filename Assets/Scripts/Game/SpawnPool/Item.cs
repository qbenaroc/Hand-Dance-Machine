using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	[Header("Score Values")]
	public int score = 0;
	public int lowScore = 1;
	public int perfectScore = 2;

	[Header("Interactable Attributes")]
	public bool iced = false;
	public bool grabbable = false;
	public bool fingerable = false;

	[Header("Glow Material")]
	[SerializeField]
	private Material mat;
	private Material baseMat;
	private MeshRenderer meshRenderer;

	[HideInInspector] public bool _activated = true;

	private void OnEnable()
	{
		//_startPos = transform.position;
		meshRenderer = GetComponent<MeshRenderer>();
		baseMat = meshRenderer.material;
	}

	void FixedUpdate()
	{
	}

	public void Glow() => meshRenderer.material = mat;

	public void StopGlow() => meshRenderer.material = baseMat;

	public void Die()
	{
		if (score == 0)
		{
			AnimationManager._i.PlayAnim(AnimationManager.Anim.Bad, transform.position);
			AudioManager._i.Play2DSound(AudioManager.Sound.LateTiming);
			ScoreManager.i.Reset();

		}
		else if (score == perfectScore)
			AnimationManager._i.PlayAnim(AnimationManager.Anim.Perfect, transform.position);
		else if (score == lowScore)
			AnimationManager._i.PlayAnim(AnimationManager.Anim.Ok, transform.position);
		ScoreManager.i.AddPoints(score);
		PoolManager._i.ReturnToPoolParentage(this.gameObject);
	}
}