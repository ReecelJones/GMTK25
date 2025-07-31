using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int width, height; //Width and Height of grid - change these values to change grid size
    [SerializeField] private Tile tilePrefab;

    [SerializeField] private Transform cam;


    private Dictionary<Vector2, Tile> tiles; //Dictionary of Tiles spawned in grid

    private void Start()
    {
        GenerateGrid();
    }

    /// <summary>
    /// Will spawn tilePrefab a X Y location dependant place in for loop based on width + height of grid
    /// Adds to dictionary to allow information to be drawn when tile accessed.
    /// Moves camera to centre of grid
    /// </summary>
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity); //Places tilePrefab in grid pattern
                spawnedTile.name = $"Tile {x} {y}"; //Renames tile to X Y pos

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0); //Returns bool if tile is offset
                spawnedTile.Init(isOffset);

                tiles[new Vector2(x, y)] = spawnedTile; //Adds tile to dictonary to allow to access tile
            }
        }

        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10); //Places camera in centre of grid
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
