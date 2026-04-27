using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartingPieceConfig
{
    public BasePiece prefab;
    [Range(0, 7)] public int startX;
    [Range(0, 7)] public int startY;
    public Team Team;
}

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 8;
    public int height = 8;
    public float tileSize = 1f;

    [Header("Prefabs")]
    public Tile tilePrefab;

    [Header("Initial Setup")]
    public List<StartingPieceConfig> startingPieces = new List<StartingPieceConfig>();

    private Dictionary<Vector2Int, Tile> tiles;

    void Start()
    {
        GenerateGrid();
        SpawnStartingPieces();
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
                spawnedTile.name = $"{GetColumnName(x)}{y + 1}";

                bool isLightSquare = (x + y) % 2 != 0;
                spawnedTile.Init(new Vector2Int(x, y), isLightSquare);

                tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }
    }

    void SpawnStartingPieces()
    {
        if (startingPieces == null) return;

        foreach (var config in startingPieces)
        {
            float spawnX = (config.startX - (width / 2f - 0.5f)) * tileSize;
            float spawnY = (config.startY - (height / 2f - 0.5f)) * tileSize;
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

            BasePiece spawnedPiece = Instantiate(config.prefab, spawnPosition, Quaternion.identity, transform);
            spawnedPiece.Team = config.Team;
            spawnedPiece.GridPosition = new Vector2Int(config.startX, config.startY);

            Tile tile = GetTileAtPosition(spawnedPiece.GridPosition);
            if (tile != null)
            {
                tile.OccupyingPiece = spawnedPiece;
            }
            else
            {
                Debug.LogWarning($"Trying to spawn piece at invalid tile {config.startX}, {config.startY}");
            }
        }
    }

    private string GetColumnName(int index)
    {
        string columnName = "";
        while (index >= 0)
        {
            columnName = (char)('a' + (index % 26)) + columnName;
            index = (index / 26) - 1;
        }
        return columnName;
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
