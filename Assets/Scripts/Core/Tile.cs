using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color darkColor = Color.gray;
    [SerializeField] private MeshRenderer meshRenderer;

    public Vector2Int GridPosition { get; private set; }
    public BasePiece OccupyingPiece { get; set; }

    public void Init(Vector2Int gridPos, bool isLightSquare)
    {
        GridPosition = gridPos;
        if (meshRenderer != null)
        {
            meshRenderer.material.color = isLightSquare ? lightColor : darkColor;
        }
    }
}
