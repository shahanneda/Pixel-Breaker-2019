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

    public GameObject pauseMenu;

    private TileManager tileManager;
    private MusicManager musicManager;

    private bool paused;

    private void Start()
    {
        string settingsPath = Application.persistentDataPath + "/Settings.json";

        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            settingsFile = JsonUtility.FromJson<SettingsFile>(json);
        }

        tileManager = FindObjectOfType<TileManager>();
        musicManager = FindObjectOfType<MusicManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        paused = true;

        tileManager.isPlaying = false;
        musicManager.ToggleMusic(false);

        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        paused = false;

        tileManager.isPlaying = true;
        musicManager.ToggleMusic(true);

        pauseMenu.SetActive(false);
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

