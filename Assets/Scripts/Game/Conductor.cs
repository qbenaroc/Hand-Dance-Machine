using PathCreation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor _i;

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    [Space(10)]
    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //an AudioSource attached to this GameObject that will play the music.
    private AudioSource musicSource;

    [Header("Song Options")]
    [Space(10)]
    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //The start offset of the music
    public float _musicBeatOffset;

    public float beatsShownInAdvance = 3;

    //the index of the next note to be spawned
    private int i = 0;

    [Header("Path Objects")]
    [Space(10)]
    public List<PathCreator> pathList = new List<PathCreator>();
    public List<Transform> startList = new List<Transform>();

    private void Awake()
    {
        _i = this;
    }

	void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        musicSource.PlayScheduled(dspSongTime + _musicBeatOffset);
    }

    void Update()
    {
        //determine how many seconds since the song started
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - _musicBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;

        if (i < LevelManager._i.itemToSpawnList.Count && LevelManager._i.itemToSpawnList[i].spawnAtBeat < songPositionInBeats + beatsShownInAdvance)
        {
            GameObject cloneBeat = PoolManager._i.GetPooledObject(LevelManager._i.itemToSpawnList[i].tag.ToString());
            FollowPath newFollow = cloneBeat.GetComponent<FollowPath>();

            newFollow.pathCreator = pathList[LevelManager._i.itemToSpawnList[i].startId];
            cloneBeat.transform.position = startList[LevelManager._i.itemToSpawnList[i].startId].position;
            newFollow.atBeat = LevelManager._i.itemToSpawnList[i].spawnAtBeat;
            newFollow.end = EndOfPathInstruction.Stop;
            newFollow.timeToReach = beatsShownInAdvance * secPerBeat;
            cloneBeat.transform.parent = newFollow.pathCreator.transform;
            cloneBeat.SetActive(true);
            i++;
        }
    }

    public void PitchSongNormal()
    {
        musicSource.pitch = 1;
    }

    public void PitchSongForward(float value)
    {
        AudioListener.pause = false;
        musicSource.pitch = 1 + value;
    }
    public void PitchSongBackward(float value)
    {
        AudioListener.pause = false;
        musicSource.pitch = value - 1;
    }
}