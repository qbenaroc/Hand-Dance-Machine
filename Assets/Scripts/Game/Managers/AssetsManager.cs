using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ObjectPoolItem
{
	public GameObject				objectToPool;
	public int						amountToPool;
	public bool						shouldExpand;
}

[Serializable] public class AnimationClip
{
	public AnimationManager.Anim	anim;
	public GameObject				animPrefab;
}

[Serializable] public class SoundAudioClip
{
	public AudioManager.Sound		sound;
	public AudioClip				audioClip;
	[Range(0f, 1f)]
	public float					volume = 0.1f;
}

[Serializable] public class SongAudioClip
{
	public AudioManager.Song		song;
	public AudioClip				audioClip;
	[Range(0f, 1f)]
	public float					volume = 0.1f;
}

public class AssetsManager : MonoBehaviour
{
	public static AssetsManager sharedInstance;

	[Header("Pool of Items")]
	[Space(10)]
	public List<ObjectPoolItem> itemsToPool;

	[Header("Songs")]
	[Space(10)]
	public SongAudioClip[] songAudioClipArray;

	[Header("Sound Design Audio Clip")]
	[Space(10)]
	public SoundAudioClip[] soundAudioClipArray;

	[Header("Animations")]
	[Space(10)]
	public AnimationClip[] animationClipArray;

	private void Awake() => sharedInstance = this;
}
