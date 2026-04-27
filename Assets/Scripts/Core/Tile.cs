using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
public class Tile : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color lightColor = Color.white;
    [SerializeField] private Color darkColor = Color.gray;
    [SerializeField] private Color highlightColor = Color.green;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color baseColor;

    public Vector2Int GridPosition { get; private set; }
    public BasePiece OccupyingPiece { get; set; }

    public void Init(Vector2Int gridPos, bool isLightSquare)
    {
        GridPosition = gridPos;
        baseColor = isLightSquare ? lightColor : darkColor;
        
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = baseColor;
        }
    }

    public void Highlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = highlightColor;
        }
    }

    public void RevertHighlight()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = baseColor;
        }
    }
}
