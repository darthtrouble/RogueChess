using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy
}

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public abstract class BasePiece : MonoBehaviour
{
    [Header("Base Stats")]
    public int baseMaxHealth = 10;
    public int baseAttackPower = 2;
    public int baseMovementRange = 1;

    [Header("Current Stats")]
    public int maxHealth;
    public int currentHealth;
    public int attackPower;
    public int movementRange;

    [Header("Positioning")]
    public Team Team;
    public Vector2Int GridPosition { get; set; }

    protected List<PieceModifier> activeModifiers = new List<PieceModifier>();

    protected virtual void Awake()
    {
        currentHealth = baseMaxHealth;
        RecalculateStats();
    }

    protected virtual void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = (Team == Team.Player) ? Color.blue : Color.red;
        }
    }

    public void AddModifier(PieceModifier modifier)
    {
        // Instantiate so we don't modify the original ScriptableObject asset
        PieceModifier instance = Instantiate(modifier);
        activeModifiers.Add(instance);
        
        instance.ApplyModifier(this);
        RecalculateStats();
    }

    public void RemoveModifier(PieceModifier modifier)
    {
        if (activeModifiers.Contains(modifier))
        {
            modifier.RemoveModifier(this);
            activeModifiers.Remove(modifier);
            RecalculateStats();
        }
    }

    public virtual void RecalculateStats()
    {
        // Reset to bases (modifiers directly alter bases in ApplyModifier)
        maxHealth = baseMaxHealth;
        attackPower = baseAttackPower;
        movementRange = baseMovementRange;
        
        // Clamp health in case max health decreased
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Capture(Tile targetTile)
    {
        if (targetTile.OccupyingPiece != null)
        {
            Destroy(targetTile.OccupyingPiece.gameObject);
            targetTile.OccupyingPiece = null;
        }
    }

    public abstract List<Vector2Int> GetValidMoves(GridManager grid);
}
