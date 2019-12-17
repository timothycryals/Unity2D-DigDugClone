using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    [SerializeField]
    private Text currentScore;
    [SerializeField]
    private Text highScore;
    [SerializeField]
    private Text currentRound;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Subscribe to the PlayerScore class to get score data for updating the GUI
        PlayerScore.UpdateScoreGUI += UpdateScore;

        //Subscribe to the GameManager class to get round data for updating the GUI
        GameManager.instance.UpdateRoundNumberGUI += UpdateRound;

        UpdateScore(0, PlayerScore.highScore);
    }

    private void OnDestroy()
    {
        PlayerScore.UpdateScoreGUI -= UpdateScore;
        GameManager.instance.UpdateRoundNumberGUI -= UpdateRound;
    }

    //Update the score text box according to the player's current scores
    private void UpdateScore(int CurrentScore, int HighScore)
    {
        currentScore.text = CurrentScore.ToString();
        highScore.text = HighScore.ToString();
    }

    //Update the round according to the GameManager
    private void UpdateRound(int roundNumber)
    {
        currentRound.text = roundNumber.ToString();
    }
}
