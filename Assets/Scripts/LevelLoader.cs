using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    public Tile grassTile, rockTile;
    public Tile enemyTile; // Optional: Use null for now
    public PlayerUnit playerPrefab;
    public EnemyUnit enemyPrefab;

    public Transform cam;

    [Header("Level File")]
    public LevelData levelData;
    public List<LevelData> levels;
    private int currentLevelIndex = 0;


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
            // or GameOver
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

        tiles = new Dictionary<Vector2, Tile>();

        string[] lines = levelData.levelTextFile.text.Trim().Split('\n');
        int height = lines.Length;
        int width = lines[0].Split(',').Length;

        for (int y = 0; y < height; y++)
        {
            string[] chars = lines[y].Trim().Split(',');
            for (int x = 0; x < chars.Length; x++)
            {
                string symbol = chars[x].Trim();
                Tile tilePrefab = null;

                switch (symbol)
                {
                    case "G":
                        tilePrefab = grassTile;
                        break;
                    case "R":
                        tilePrefab = rockTile;
                        break;
                    case "E":
                        tilePrefab = grassTile; // base tile + spawn enemy
                        break;
                    case "P":
                        tilePrefab = grassTile; // base tile + spawn player
                        break;
                    default:
                        tilePrefab = grassTile; // fallback
                        break;
                }

                Vector2 position = new Vector2(x, height - 1 - y); // Flip Y

                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x},{y}";

                bool isOffset = (x + y) % 2 == 1;
                spawnedTile.Init(isOffset);

                tiles[position] = spawnedTile;
                GridManager.Instance.RegisterTile(position, spawnedTile);


                if (symbol == "P")
                {
                    playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);
                    playerInstance.SetPosition(position);
                    GameManager.instance.playerUnit = playerInstance;
 

                    if (levelData.availableActions != null)
                    {
                        playerInstance.actionLoop = new List<UnitAction>(levelData.availableActions);
                    }

                    if (playerUIHandler != null)
                    {
                        playerUIHandler.playerUnit = playerInstance;
                        playerUIHandler.RefreshUI();
                    }
                }

                // Optional: instantiate enemy units
                if (symbol == "E")
                {
                    enemyInstance = Instantiate(enemyPrefab, position, Quaternion.identity);
                }
            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);

        GameManager.instance.UpdateGameState(GameState.SetActions);
    }

    private void ClearCurrentLevel()
    {
        // Destroy all previous tiles
        foreach (var tile in FindObjectsOfType<Tile>())
        {
            Destroy(tile.gameObject);
        }

        // Destroy player if exists
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
            playerInstance = null;
        }

        // Destroy enemies
        foreach (var enemy in FindObjectsOfType<EnemyUnit>())
        {
            Destroy(enemy.gameObject);
        }

        // Clear grid tiles
        GridManager.Instance.ClearAllTiles();
    }
}