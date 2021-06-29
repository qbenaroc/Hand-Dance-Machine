using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager i { get; private set; }

    public int score = 0;
    public int highScore = 0;
    private string highScoreKey = "HighScoreKey";     //string call highScore PlayerPrefs
    private string scoreKey = "ScoreKey";             //string call score PlayerPrefs

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.SetInt(scoreKey, score);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        PlayerPrefs.SetInt(scoreKey, score);
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
        }
    }

    public void AddPoints (int pointsToAdd)
    {
        score += pointsToAdd;
    }

    public void Reset()
    {
        score = 0;
    }
}
