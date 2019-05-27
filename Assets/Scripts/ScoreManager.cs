using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private int score = 0;

    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score;
    }
}
