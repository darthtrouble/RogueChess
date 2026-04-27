using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    public TurnManager turnManager;
    
    private BasePiece selectedPiece;
    private List<Tile> highlightedTiles = new List<Tile>();

    private InputAction clickAction;
    private InputAction pointerPositionAction;

    void Awake()
    {
        clickAction = new InputAction(type: InputActionType.Button, binding: "<Pointer>/press");
        pointerPositionAction = new InputAction(type: InputActionType.Value, binding: "<Pointer>/position");
        pointerPositionAction.expectedControlType = "Vector2";
    }

    void OnEnable()
    {
        clickAction.Enable();
        pointerPositionAction.Enable();
        clickAction.performed += OnClick;
    }

    void OnDisable()
    {
        clickAction.Disable();
        pointerPositionAction.Disable();
        clickAction.performed -= OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (turnManager == null || turnManager.CurrentState != TurnState.PlayerTurn)
        {
            return;
        }

        Vector2 screenPosition = pointerPositionAction.ReadValue<Vector2>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            BasePiece clickedPiece = hit.collider.GetComponent<BasePiece>();
            Tile clickedTile = hit.collider.GetComponent<Tile>();

            // If the player clicks a piece
            if (clickedPiece != null)
            {
                if (clickedPiece.Team == Team.Player && turnManager.CurrentState == TurnState.PlayerTurn)
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
        selectedPiece.Capture(targetTile);

        // Update piece data
        selectedPiece.GridPosition = targetTile.GridPosition;
        selectedPiece.transform.position = targetTile.transform.position;
        targetTile.OccupyingPiece = selectedPiece;

        // Cleanup selection
        ClearHighlights();
        selectedPiece = null;

        if (turnManager != null)
        {
            turnManager.EndTurn();
        }
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
