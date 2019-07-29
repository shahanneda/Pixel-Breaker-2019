using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    public int Score { get; private set; } = 0;

    private ScoreTextEffect scoreTextEffect;

    private void Start()
    {
        scoreTextEffect = FindObjectOfType<ScoreTextEffect>();
    }

    public void AddScore(int addScore)
    {
        Score += addScore;
        scoreText.text = Score.ToString();

        scoreTextEffect.DisplayScore(addScore);
    }
}
