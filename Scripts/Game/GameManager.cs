using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MovementDirection { LEFT, RIGHT, UP, DOWN }


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private const string GAME_SCENE_NAME = "main";

    [SerializeField]
    private GameObject gridPrefab;
    [SerializeField]
    private GameObject tunnelPrefab;
    [SerializeField]
    private GameObject playerPrefab;

    public event Action<int> UpdateRoundNumberGUI;

    public GameObject player;
    public Grid grid;

    public bool roundInProgress;

    private int _roundNumber;
    public int roundNumber
    {
        get
        {
            return _roundNumber;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerScore.LoadScores();
        roundInProgress = true;
        _roundNumber = 1;
        grid = GameObject.Find("PathfindingGrid").GetComponent<Grid>();
        StartCoroutine(UpdateGrid());
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start()
    {
        UpdateRoundNumberGUI(_roundNumber);
    }

    private void LateUpdate()
    {
        if (!roundInProgress)
        {
            UpdateRound();
        }
    }

    public void OnSceneChanged(Scene prevScene, Scene newScene)
    {
        if(newScene.name == "menu")
        {
            Player.isPlayerDead = false;
            Destroy(EnemySpawner.Instance.gameObject);
            PlayerScore.score = 0;
            roundInProgress = false;
            _roundNumber = 1;
        }
        else if(newScene.name == "main")
        {
            grid = GameObject.Find("PathfindingGrid").GetComponent<Grid>();
            roundInProgress = true;
        }
    }

    private void UpdateRound()
    {
        roundInProgress = false;
        _roundNumber += 1;
        UpdateRoundNumberGUI(_roundNumber);
        PlayerScore.SaveScores();
        SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Single);
        roundInProgress = true;

    }

    private IEnumerator UpdateGrid()
    {
        while (roundInProgress)
        {
            yield return new WaitForSeconds(1f);
            grid.UpdateGrid();
            foreach (Enemy e in Enemy.enemies)
            {
                e.UpdatePathfinderGrid(grid);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
}
