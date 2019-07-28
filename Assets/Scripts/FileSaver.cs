using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileSaver : MonoBehaviour
{
    public static void Save(int highScore)
    {
        GameSave gameSave = new GameSave(highScore);

        string path = Application.persistentDataPath + "/PixelBreakerSave.json";
        string json = JsonUtility.ToJson(gameSave);
        File.WriteAllText(path, json);
    }

    public static GameSave Load()
    {
        string path = Application.persistentDataPath + "/PixelBreakerSave.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameSave>(json);
        }
        else
        {
            return new GameSave();
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
