using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager _i { get; private set; }

	public enum Song
	{
		Song1,
		Song2
	}

    public enum Sound
    {
        LateTiming,
        MidCombo
    }

    private static GameObject oneShotSoundGameObject;
    private static AudioSource oneShotAudioSource;


	private void Awake() => _i = this;

	private void Update()
	{
	}

	public void Play3DSound(Sound sound, Vector3 position)
    {
        GameObject soundGameObject = new GameObject("3D Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

		soundGameObject.transform.parent = transform;
        soundGameObject.transform.position = position;
        audioSource.clip = GetAudioClip(sound);
        audioSource.volume = GetAudioVolume(sound);
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;
        audioSource.Play();
        Destroy(soundGameObject, audioSource.clip.length);
    }

    public void Play2DSound(Sound sound)
    {
        if (oneShotSoundGameObject == null)
        {
            oneShotSoundGameObject = new GameObject("One Shot Sound");
            oneShotAudioSource = oneShotSoundGameObject.AddComponent<AudioSource>();
			oneShotSoundGameObject.transform.parent = transform;
        }
        oneShotAudioSource.clip = GetAudioClip(sound);
        oneShotAudioSource.volume = GetAudioVolume(sound);
        oneShotAudioSource.PlayOneShot(oneShotAudioSource.clip);
		Destroy(oneShotSoundGameObject, oneShotAudioSource.clip.length);
	}

	private AudioClip GetSongAudioClip(Song song)
	{
		foreach (SongAudioClip songAudioClip in AssetsManager.sharedInstance.songAudioClipArray)
		{
			if (songAudioClip.song == song)
				return songAudioClip.audioClip;
		}
		Debug.LogError("Song" + song + " not found");
		return null;
	}

	private AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in AssetsManager.sharedInstance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
                return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound" + sound + " not found");
        return null;
    }

	private float GetSongAudioVolume(Song song)
	{
		foreach (SongAudioClip songAudioClip in AssetsManager.sharedInstance.songAudioClipArray)
		{
			if (songAudioClip.song == song)
				return songAudioClip.volume;
		}
		Debug.LogError("Volume" + song + " not found");
		return 0;
	}

	private float GetAudioVolume(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in AssetsManager.sharedInstance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
                return soundAudioClip.volume;
        }
        Debug.LogError("Volume" + sound + " not found");
        return 0;
    }
}