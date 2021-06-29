using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public static TimeControl _i;

    [HideInInspector] public bool gameIsPaused;
    public float keyDelay = 1f;
    private float _timePassed = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        _i = this;
    }

    // Update is called once per frame
    void Update()
    {
        _timePassed += Time.unscaledDeltaTime;
    }

    public void PauseGame()
	{
        if (_timePassed >= keyDelay)
        {
            gameIsPaused = !gameIsPaused;
            _timePassed = 0f;

            if (gameIsPaused)
            {
                StopGameTime();
            }
            else
            {
                StartGameTime();
            }
        }
    }

    public void StartGameTime()
    {
        Conductor._i.PitchSongNormal();
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    public void StopGameTime()
	{
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void ForwardGame(float value)
    {
        Time.timeScale = 1 + value;
        Conductor._i.PitchSongForward(value);
    }

    public void BackwardGame(float value)
	{
        Conductor._i.PitchSongBackward(value);
    }
}