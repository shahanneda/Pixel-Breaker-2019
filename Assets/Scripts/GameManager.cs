using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;

    public void GameOver()
    {
        FindObjectOfType<TileManager>().isPlaying = false;
        FindObjectOfType<TileManager>().enabled = false;

        gameOverScreen.SetActive(true);

        int score = FindObjectOfType<ScoreManager>().Score;
        int highScore = FileSaver.Load().highScore;

        if (score > highScore)
        {
            highScore = score;
            FileSaver.Save(highScore);
        }

        gameOverScoreText.text += score.ToString();
        gameOverHighScoreText.text += highScore.ToString();
    }
}

