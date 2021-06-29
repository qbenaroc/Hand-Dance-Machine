using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreGUI : MonoBehaviour
{
    private int highScore;
    TextMeshProUGUI highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        highScore = ScoreManager.i.highScore;
        highScoreText.text = highScore.ToString();
    }
}
