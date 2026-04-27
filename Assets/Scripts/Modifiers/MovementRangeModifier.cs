using UnityEngine;

[CreateAssetMenu(fileName = "MovementRangeModifier", menuName = "Modifiers/Movement Range")]
public class MovementRangeModifier : PieceModifier
{
    public int rangeBonus = 1;

    public override void ApplyModifier(BasePiece target)
    {
        target.baseMovementRange += rangeBonus;
        Debug.Log($"Applied MovementRangeModifier: {target.name} gained {rangeBonus} range.");
    }

    public override void RemoveModifier(BasePiece target)
    {
        target.baseMovementRange -= rangeBonus;
        Debug.Log($"Removed MovementRangeModifier from {target.name}.");
    }
}
