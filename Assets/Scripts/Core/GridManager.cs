using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;
    public float tileSize = 1f;

    [Header("Prefabs")]
    public Tile tilePrefab;

    private Dictionary<Vector2Int, Tile> tiles;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2Int, Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float spawnX = (x - (width / 2f - 0.5f)) * tileSize;
                float spawnY = (y - (height / 2f - 0.5f)) * tileSize;
                Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

                var spawnedTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity, transform);
                spawnedTile.name = $"Tile {x} {y}";

                bool isLightSquare = (x + y) % 2 != 0;
                spawnedTile.Init(new Vector2Int(x, y), isLightSquare);

                tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if (tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
}
