using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    private int score = 0;

    private ScoreTextEffect scoreTextEffect;

    private void Start()
    {
        scoreTextEffect = FindObjectOfType<ScoreTextEffect>();
    }

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();

        scoreTextEffect.DisplayScore(addScore);
    }
}
