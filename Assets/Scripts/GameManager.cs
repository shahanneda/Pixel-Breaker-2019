using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverScoreText;

    public void GameOver()
    {
        FindObjectOfType<TileManager>().isPlaying = false;
        FindObjectOfType<TileManager>().enabled = false;

        gameOverScreen.SetActive(true);
        gameOverScoreText.text += FindObjectOfType<ScoreManager>().Score.ToString();
    }
}
