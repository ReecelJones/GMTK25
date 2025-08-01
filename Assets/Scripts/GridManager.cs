using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private PlayerUnit playerPrefab;
    private PlayerUnit playerInstance;

    [SerializeField] private int width, height; //Width and Height of grid - change these values to change grid size
    [SerializeField] private Tile grassTile, rockTile;

    [SerializeField] private Transform cam;


    private Dictionary<Vector2, Tile> tiles; //Dictionary of Tiles spawned in grid
    private void Awake()
    {
        Instance = this;
    }

    public void RegisterTile(Vector2 position, Tile tile)
    {
        if (tiles == null)
        {
            tiles = new Dictionary<Vector2, Tile>();
        }

        tiles[position] = tile;
    }
    /// <summary>
    /// Will spawn tilePrefab a X Y location dependant place in for loop based on width + height of grid
    /// Adds to dictionary to allow information to be drawn when tile accessed.
    /// Moves camera to centre of grid
    /// </summary>
    /// 
    public void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        var randomTile = Random.Range(0, 6) == 3 ? rockTile : grassTile;
        //        var spawnedTile = Instantiate(randomTile, new Vector3(x, y), Quaternion.identity); //Places tilePrefab in grid pattern
        //        spawnedTile.name = $"Tile {x} {y}"; //Renames tile to X Y pos

        //        var isOffset = (x + y) % 2 == 1; //Returns bool if tile is offset
        //        spawnedTile.Init(isOffset);

        //        tiles[new Vector2(x, y)] = spawnedTile; //Adds tile to dictonary to allow to access tile
        //    }
        //}

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10); //Places camera in centre of grid

        //// Spawn player at a valid tile (e.g., bottom left)
        //Vector2 spawnPosition = new Vector2(0, 0);
        //if (tiles[spawnPosition] is GrassTile)
        //{
        //    playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        //}

        GameManager.instance.UpdateGameState(GameState.SetActions);
    }

    /// <summary>
    /// Will return Tile at position param if available
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Tile GetTileAtPosition(Vector2 position)
    {
        if (tiles.TryGetValue(position, out var tile)) //If tile is available, will return tile (AKA - run function when enemy move to attach tile to enemy) - Hopefully
        {
            return tile;
        }

        return null;
    }
}