using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private ResultHandler resultHandler;

    public GameState GameState;
    public int objectiveKills;

    public bool playerIsDead;

    public static event Action<GameState> OnGameStateChanged;

    public List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
    public PlayerUnit playerUnit;

    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private PlayerActionHandler playerActionHandler;

    private List<UnitAction> savedPlayerActions;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateGameState(GameState.GenerateGrid);
        resultHandler = FindFirstObjectByType<ResultHandler>();
    }

    public void UpdateGameState(GameState newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameState.GenerateGrid:
                levelLoader.LoadLevelFromText();
                objectiveKills = enemyUnits.Count;
                break;
            case GameState.SetActions:
                HandleSetActions();
                break;
            case GameState.PlayerTurn:
                StartPlayerTurn();
                break;
            case GameState.EnemyTurn:
                EnemyTurn();
                break;
            case GameState.EndResult:
                if (playerIsDead)
                {
                    HandleResult(false);
                }
                else
                {
                    HandleResult(true);
                }
                break;
            case GameState.Reset:
                HandleResetLevel();
                break;
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSetActions()
    {
        playerActionHandler.RefreshUI();
    }

    private void EnemyTurn()
    {
        foreach (var enemy in enemyUnits)
        {
            enemy.PlayActionLoop();
        }

        UpdateGameState(GameState.PlayerTurn); // Or queue up delays/animations
    }

    public void StartPlayerTurn()
    {
        playerUnit.PlayActionLoop();
    }

    public void EndPlayerTurn()
    {
        UpdateGameState(GameState.EnemyTurn);

        //// Start enemy turn next, for example:
        //StartCoroutine(HandleEnemyTurn());
        EnemyTurn();
    }

    private IEnumerator RunPlayerTurn()
    {
        PlayerUnit player = FindFirstObjectByType<PlayerUnit>();

        if (player != null)
        {
            player.ExecuteNextAction(); // this uses the actionLoop
            yield return new WaitForSeconds(0.5f); // optional delay
        }

        UpdateGameState(GameState.EnemyTurn); // or wait for input/reset option
    }

    public void HandleResetLevel()
    {
        // 1. Save current action order
        if (playerUnit != null && playerUnit.actionLoop != null)
            savedPlayerActions = new List<UnitAction>(playerUnit.actionLoop);
        else
            savedPlayerActions = null;

        // 2. Destroy current player
        if (playerUnit != null)
        {
            Destroy(playerUnit.gameObject);
            playerUnit = null;
        }

        // 3. Clear previous enemies
        foreach (var enemy in enemyUnits)
        {
            if (enemy != null)
                Destroy(enemy.gameObject);
        }
        enemyUnits.Clear();
        objectiveKills = 0;

        // 4. Reload the level
        levelLoader.LoadLevelFromText();

        // 5. Wait a frame and reassign player
        StartCoroutine(ApplySavedActionsAfterReload());
    }

    public void OnEnemyKilled(EnemyUnit enemy)
    {
        if (enemyUnits.Contains(enemy))
            enemyUnits.Remove(enemy);

        objectiveKills--;

        if (objectiveKills <= 0)
        {
            UpdateGameState(GameState.EndResult);
        }
    }

    public void HandleResult(bool PlayerWin)
    {
        resultHandler.didPlayerWin = PlayerWin;
        resultHandler.DisplayEndScreen();
    }

    private IEnumerator ApplySavedActionsAfterReload()
    {
        // Wait for one frame so the level is fully loaded and player is instantiated
        yield return null;

        playerUnit = FindFirstObjectByType<PlayerUnit>();

        if (playerUnit != null)
        {
            // Restore saved actions
            if (savedPlayerActions != null)
                playerUnit.actionLoop = new List<UnitAction>(savedPlayerActions);

            // Reconnect PlayerActionHandler (in case it's tied to old reference)
            if (playerActionHandler != null)
            {
                playerActionHandler.playerUnit = playerUnit;
                playerActionHandler.RefreshUI();
            }
            else
            {
                Debug.LogWarning("PlayerActionHandler is not assigned in GameManager.");
            }
        }
        else
        {
            Debug.LogError("PlayerUnit not found after level reload.");
        }

        UpdateGameState(GameState.SetActions);
    }

  
}

public enum GameState
{
    GenerateGrid,
    SetActions,
    PlayerTurn,
    EnemyTurn,
    EndResult,
    Reset
}
