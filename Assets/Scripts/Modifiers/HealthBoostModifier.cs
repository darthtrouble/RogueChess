using UnityEngine;

[CreateAssetMenu(fileName = "HealthBoostModifier", menuName = "Modifiers/Health Boost")]
public class HealthBoostModifier : PieceModifier
{
    public int healthBonus = 5;

    public override void ApplyModifier(BasePiece target)
    {
        target.baseMaxHealth += healthBonus;
        target.currentHealth += healthBonus;
        Debug.Log($"Applied HealthBoost: {target.name} gained {healthBonus} HP.");
    }

    public override void RemoveModifier(BasePiece target)
    {
        target.baseMaxHealth -= healthBonus;
        // Current health clamping happens in BasePiece.RecalculateStats()
        Debug.Log($"Removed HealthBoost from {target.name}.");
    }
}
