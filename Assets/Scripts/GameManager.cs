using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverScoreText;
    public Text gameOverHighScoreText;

    public SettingsFile settingsFile;

    private void Start()
    {
        string settingsPath = Application.persistentDataPath + "/Settings.json";

        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            settingsFile = JsonUtility.FromJson<SettingsFile>(json);
        }
    }

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

