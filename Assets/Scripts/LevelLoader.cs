using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    public Tile grassTile, rockTile;
    public Tile enemyTile;
    public PlayerUnit playerPrefab;
    public EnemyUnit enemyPrefab;
    public GameObject tutorialPrompt;

    public Transform cam;

    [Header("Level File")]
    public LevelData levelData;
    public List<LevelData> levels;
    private int currentLevelIndex = 0;

    private bool isTutorial = true;
    private Dictionary<Vector2, Tile> tiles;
    private PlayerUnit playerInstance;
    private EnemyUnit enemyInstance;
    public PlayerActionHandler playerUIHandler;

    public void LoadCurrentLevel()
    {
        if (levels == null || levels.Count == 0 || currentLevelIndex >= levels.Count)
        {
            Debug.LogError("No levels available or index out of range.");
            return;
        }

        levelData = levels[currentLevelIndex];
        LoadLevelFromText();
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Count)
        {
            LoadCurrentLevel();
        }
        else
        {
            Debug.Log("No more levels! You win!");
        }
    }

    public void LoadLevelFromText()
    {
        ClearCurrentLevel();

        if (levelData.levelTextFile == null)
        {
            Debug.LogError("No level text file assigned!");
            return;
        }

        if (isTutorial)
        {
            tutorialPrompt.SetActive(true);
            isTutorial = false;
        }
        else
        {
            Destroy(tutorialPrompt);
        }

        tiles = new Dictionary<Vector2, Tile>();

        string[] lines = levelData.levelTextFile.text.Trim().Split('\n');
        StartCoroutine(AnimateGridSpawn(lines));
    }

    private IEnumerator AnimateGridSpawn(string[] lines)
    {
        int height = lines.Length;
        int width = lines[0].Split(',').Length;

        tiles = new Dictionary<Vector2, Tile>();

        // Center the camera BEFORE spawning tiles
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        for (int y = 0; y < height; y++)
        {
            string[] symbols = lines[y].Trim().Split(',');

            for (int x = 0; x < symbols.Length; x++)
            {
                string symbol = symbols[x].Trim();
                Tile tilePrefab = GetTilePrefab(symbol);

                Vector2 position = new Vector2(x, height - 1 - y);

                // Fly in from above
                Vector3 spawnPos = new Vector3(position.x, position.y + 5f, 0);
                Tile spawnedTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                spawnedTile.name = $"Tile {x},{y}";

                bool isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                tiles[position] = spawnedTile;
                GridManager.Instance.RegisterTile(position, spawnedTile);

                LeanTween.moveY(spawnedTile.gameObject, position.y, 0.3f).setEaseOutCubic();

                //created new voids for ease
                if (symbol == "P") SpawnPlayer(position);
                if (symbol == "E") SpawnEnemy(position);

                yield return new WaitForSeconds(0.05f);
            }
        }

        GameManager.instance.UpdateGameState(GameState.SetActions);
    }

    private Tile GetTilePrefab(string symbol)
    {
        switch (symbol)
        {
            case "G": return grassTile;
            case "R": return rockTile;
            case "E": return grassTile;
            case "P": return grassTile;
            default: return grassTile;
        }
    }

    private void SpawnPlayer(Vector2 position)
    {
        playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);
        playerInstance.SetPosition(position);
        GameManager.instance.playerUnit = playerInstance;

        if (levelData.availablePlayerActions != null)
        {
            playerInstance.actionLoop = new List<UnitAction>(levelData.availablePlayerActions);
        }

        if (playerUIHandler != null)
        {
            playerUIHandler.playerUnit = playerInstance;
            playerUIHandler.RefreshUI();
        }
    }

    private void SpawnEnemy(Vector2 position)
    {
        enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    private void ClearCurrentLevel()
    {
        foreach (var tile in FindObjectsByType<Tile>(FindObjectsSortMode.None))
        {
            Destroy(tile.gameObject);
        }

        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
            playerInstance = null;
        }

        foreach (var enemy in FindObjectsByType<EnemyUnit>(FindObjectsSortMode.None))
        {
            Destroy(enemy.gameObject);
        }

        GridManager.Instance.ClearAllTiles();
    }

    private void Update()
    {
        if (isTutorial && GameManager.instance.GameState == GameState.EndResult)
        {
            Destroy(tutorialPrompt);
        }
    }
}