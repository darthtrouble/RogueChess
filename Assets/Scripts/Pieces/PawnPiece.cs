using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : BasePiece
{
    public override List<Vector2Int> GetValidMoves(GridManager grid)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        
        int direction = (Team == Team.Player) ? 1 : -1;
        
        // Forward movement (1 space if empty)
        Vector2Int forwardPos = new Vector2Int(GridPosition.x, GridPosition.y + direction);
        Tile forwardTile = grid.GetTileAtPosition(forwardPos);
        if (forwardTile != null && forwardTile.OccupyingPiece == null)
        {
            validMoves.Add(forwardPos);
        }

        // Diagonal captures
        Vector2Int[] diagonalPositions = new Vector2Int[]
        {
            new Vector2Int(GridPosition.x - 1, GridPosition.y + direction),
            new Vector2Int(GridPosition.x + 1, GridPosition.y + direction)
        };

        foreach (var diagPos in diagonalPositions)
        {
            Tile diagTile = grid.GetTileAtPosition(diagPos);
            if (diagTile != null && diagTile.OccupyingPiece != null)
            {
                // Can only capture enemies
                if (diagTile.OccupyingPiece.Team != this.Team)
                {
                    validMoves.Add(diagPos);
                }
            }
        }
        
        return validMoves;
    }
}
