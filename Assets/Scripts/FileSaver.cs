using UnityEngine;

public class FileSaver : MonoBehaviour
{
    public static void Save(int highScore)
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public static GameSave Load()
    {
        try
        {
            return new GameSave(PlayerPrefs.GetInt("HighScore"));
        }
        catch
        {
            return new GameSave(0);
        }
    }
}

[System.Serializable]
public class GameSave
{
    public int highScore;

    public GameSave()
    {

    }

    public GameSave(int highScore)
    {
        this.highScore = highScore;
    }
}
