using System.Collections.Generic;
using UnityEngine;

public class PawnPiece : BasePiece
{
    public override List<Vector2Int> GetValidMoves(GridManager grid)
    {
        List<Vector2Int> validMoves = new List<Vector2Int>();
        
        // Simple forward movement
        int direction = (Affiliation == PieceAffiliation.Player) ? 1 : -1;
        
        for (int i = 1; i <= movementRange; i++)
        {
            Vector2Int forwardPos = new Vector2Int(GridPosition.x, GridPosition.y + (direction * i));
            
            Tile tile = grid.GetTileAtPosition(forwardPos);
            if (tile != null && tile.OccupyingPiece == null)
            {
                validMoves.Add(forwardPos);
            }
            else
            {
                break; // Blocked by another piece or the edge of the board
            }
        }

        // Additional pawn behavior like diagonal capture can be implemented here
        
        return validMoves;
    }
}
