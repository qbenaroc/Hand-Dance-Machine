using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreGUI : MonoBehaviour
{
    private int score;
    TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        score = ScoreManager.i.score;
        scoreText.text = score.ToString();
    }
}
