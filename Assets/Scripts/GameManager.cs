using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState GameState;

    public static event Action<GameState> OnGameStateChanged;

    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    public PlayerUnit playerUnit;

    [SerializeField] private string levelFileName = "Level1.txt";
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerActionHandler playerActionHandler;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);
        playerUnit = FindFirstObjectByType<PlayerUnit>();
    }

    public void UpdateGameState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                levelLoader.LoadLevelFromText();
                break;
            case GameState.SetActions:
                HandleSetActions();
                break;
            case GameState.PlayerTurn:
                break;
            case GameState.EnemyTurn:
                break;
            case GameState.Victory:
                break;
            case GameState.Reset:
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSetActions()
    {
        playerActionHandler.RefreshUI();
        UpdateGameState(GameState.PlayerTurn);
    }

    private void EnemyTurn()
    {
        foreach (var enemy in enemyUnits)
        {
            enemy.ExecuteNextAction();
        }

        UpdateGameState(GameState.PlayerTurn); // Or queue up delays/animations
    }

    public void StartPlayerTurn()
    {
        UpdateGameState(GameState.PlayerTurn);
        playerUnit.PlayActionLoop();
    }
    public void EndPlayerTurn()
    {
        UpdateGameState(GameState.EnemyTurn);

        //// Start enemy turn next, for example:
        //StartCoroutine(HandleEnemyTurn());
    }

    private IEnumerator RunPlayerTurn()
    {
        PlayerUnit player = FindObjectOfType<PlayerUnit>();

        if (player != null)
        {
            player.ExecuteNextAction(); // this uses the actionLoop
            yield return new WaitForSeconds(0.5f); // optional delay
        }

        UpdateGameState(GameState.EnemyTurn); // or wait for input/reset option
    }
}

public enum GameState
{
    GenerateGrid,
    SetActions,
    PlayerTurn,
    EnemyTurn,
    Victory,
    Reset
}
