using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    public int Score { get; private set; } = 0;

    private TileManager tileManager;
    private ScoreTextEffect scoreTextEffect;

    private int addRowCounter;

    private void Start()
    {
        tileManager = FindObjectOfType<TileManager>();
        scoreTextEffect = FindObjectOfType<ScoreTextEffect>();
    }

    public void AddScore(int addScore)
    {
        Score += addScore;
        scoreText.text = Score.ToString();

        scoreTextEffect.DisplayScore(addScore);

        if (Mathf.FloorToInt(Score / 200) > addRowCounter)
        {
            tileManager.DecreaseTimeBetweenAddRow();
            addRowCounter++;
        }
    }
}
