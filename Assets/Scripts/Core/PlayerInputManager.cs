using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    
    private BasePiece selectedPiece;
    private List<Tile> highlightedTiles = new List<Tile>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            BasePiece clickedPiece = hit.collider.GetComponent<BasePiece>();
            Tile clickedTile = hit.collider.GetComponent<Tile>();

            // If the player clicks a piece
            if (clickedPiece != null)
            {
                if (clickedPiece.Affiliation == PieceAffiliation.Player)
                {
                    SelectPiece(clickedPiece);
                }
                else if (selectedPiece != null)
                {
                    // Attempting to attack/capture an enemy piece
                    Tile targetTile = gridManager.GetTileAtPosition(clickedPiece.GridPosition);
                    if (targetTile != null && highlightedTiles.Contains(targetTile))
                    {
                        MoveSelectedPieceTo(targetTile);
                    }
                }
                return;
            }

            // If the player clicks an empty tile
            if (clickedTile != null)
            {
                if (selectedPiece != null && highlightedTiles.Contains(clickedTile))
                {
                    MoveSelectedPieceTo(clickedTile);
                }
                else
                {
                    // Clicked an invalid tile or a tile with no piece selected
                    ClearHighlights();
                    selectedPiece = null;
                }
            }
        }
        else
        {
            // Clicked outside the board
            ClearHighlights();
            selectedPiece = null;
        }
    }

    private void SelectPiece(BasePiece piece)
    {
        ClearHighlights();
        selectedPiece = piece;

        List<Vector2Int> validMoves = piece.GetValidMoves(gridManager);
        foreach (var move in validMoves)
        {
            Tile tile = gridManager.GetTileAtPosition(move);
            if (tile != null)
            {
                tile.Highlight();
                highlightedTiles.Add(tile);
            }
        }
    }

    private void MoveSelectedPieceTo(Tile targetTile)
    {
        // Free the old tile
        Tile oldTile = gridManager.GetTileAtPosition(selectedPiece.GridPosition);
        if (oldTile != null)
        {
            oldTile.OccupyingPiece = null;
        }

        // Handle capture if an enemy piece is on the target tile
        if (targetTile.OccupyingPiece != null)
        {
            Destroy(targetTile.OccupyingPiece.gameObject);
        }

        // Update piece data
        selectedPiece.GridPosition = targetTile.GridPosition;
        selectedPiece.transform.position = targetTile.transform.position;
        targetTile.OccupyingPiece = selectedPiece;

        // Cleanup selection
        ClearHighlights();
        selectedPiece = null;

        // Note: You can optionally trigger TurnManager.EndPlayerTurn() here.
        // var turnManager = FindObjectOfType<TurnManager>();
        // if (turnManager != null) turnManager.EndPlayerTurn();
    }

    private void ClearHighlights()
    {
        foreach (var tile in highlightedTiles)
        {
            tile.RevertHighlight();
        }
        highlightedTiles.Clear();
    }
}
