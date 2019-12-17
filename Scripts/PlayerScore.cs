using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class PlayerScore
{
    //Dictionary to store the highscores. Key = round number. Value = high score.
    //private static Dictionary<int, int> HighScores;
    private static int globalHighScore = 0;

    //Broadcast to the GUIManager class to update the scores in the UI. Takes the current score and the high score as parameters.
    public static event Action<int, int> UpdateScoreGUI;

    public static int score = 0;
    public static int highScore;
    

    //Add the given amount of points to the player's current score.
    public static void AddToScore(int points)
    {
        score += points;
        //if the current score is higher than the previous high score, overwrite the high score with the current score.
        if (score > highScore)
        {
            highScore = score;
        }

        UpdateScoreGUI(score, highScore);
    }

    public static void SaveScores()
    {
        string fileName = Application.persistentDataPath + "/scores.dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(fileName, FileMode.Create);

        PlayerData data = new PlayerData();
        data.highScore = highScore;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void LoadScores()
    {
        string fileName = Application.persistentDataPath + "/scores.dat";
        if (File.Exists(fileName))
        {
            Debug.Log("Loading file");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(fileName, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            globalHighScore = data.highScore;
            Debug.Log(globalHighScore);
            highScore = globalHighScore;
        }
        else
        {
            Debug.Log("No such file");
            globalHighScore = 0;
        }
    }
}
